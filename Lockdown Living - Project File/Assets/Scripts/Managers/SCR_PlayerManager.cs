using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SCR_PlayerManager : MonoBehaviour
{
    public static SCR_PlayerManager playerManager;

    public int playerAmount;
    public int spawnedPlayers;
    private GameObject[] players;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private Text playerPrompt;

    #region Getters and Setters
    public int GetPlayerAmount() => playerAmount;
    public void SetPlayerAmount(int placeHolder) => playerAmount = placeHolder;

    public int GetSpawnedPlayers() => spawnedPlayers;
    public void SetSpawnedPlayers(int placeHolder) => spawnedPlayers = placeHolder;

    public GameObject GetPlayer(int placeHolder) => players[placeHolder];
    public void SetPlayer(int placeHolder, GameObject newPlayer) => players[placeHolder] = newPlayer;

    public GameObject GetSpawner(int placeHolder) => spawners[placeHolder];
    public void SetSpawner(int placeHolder, GameObject newSpawner) => spawners[placeHolder] = newSpawner;

    public Text GetPlayerPromptText() => playerPrompt;
    public void SetPlayerPromptText(Text newText) => playerPrompt = newText;

    public int GetPlayerIndex(GameObject player)
    {
        for(int i = 0; i < playerAmount; i++)
        {
            if (players[i] == player)
            {
                return i;
            }
        }
        return 0;
    }
    #endregion

    private void Awake()
    {        
        if (playerManager)
        {
            playerManager.StopAllCoroutines();
            ShareData();
            playerManager.StartCoroutine(playerManager.NewScene());
            Destroy(gameObject);
        }
        else
        {
            gameObject.GetComponent<PlayerInputManager>().DisableJoining();
            playerManager = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(SetUp());
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        if(spawnedPlayers < playerAmount)
        {
            players[spawnedPlayers] = player.gameObject;

            PlacePlayerOnSpawner(spawnedPlayers);

            players[spawnedPlayers].GetComponent<PlayerInput>().DeactivateInput();

            players[spawnedPlayers].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = spawners[spawnedPlayers].GetComponent<MeshRenderer>().sharedMaterial;

            spawnedPlayers++;

            player.gameObject.name = "Player " + spawnedPlayers.ToString();

            int placeholder = spawnedPlayers + 1;
            playerPrompt.text = "Player " + placeholder.ToString() + " press\n Start / Enter";

            if (spawnedPlayers == playerAmount)
            {
                playerPrompt.text = "Ready Up!";
                ToggleJoining(false);
                SCR_GameManager.gameManager.gameObject.GetComponent<SCR_MaterialPicker>().SetUp();
                SCR_GameManager.gameManager.StartGame();
            }
        }        
    }

    public void PlacePlayerOnSpawner(int player)
    {
        players[player].GetComponent<CharacterController>().enabled = false;
        if(SCR_GameManager.gameManager.GetScene() == 3)
        {
            players[player].transform.position = spawners[player].transform.position + new Vector3(0f, 2f, 0f);
        }
        else
        {
            players[player].transform.position = spawners[player].transform.position;
        }
        players[player].transform.rotation = Quaternion.Euler(spawners[player].transform.rotation.eulerAngles);
        players[player].GetComponent<CharacterController>().enabled = true;
    }

    public void ShareData()
    {
        for (int i = 0; i < 4; i++)
        {
            playerManager.SetSpawner(i, spawners[i]);
        }
    }

    IEnumerator NewScene()
    {
        yield return new WaitForSeconds(3f);
        if (SCR_GameManager.gameManager.GetScene() != 3)
        {
            for (int i = 3; i > playerAmount - 1; i--)
            {
                Destroy(spawners[i]);
            }
            yield return new WaitForSeconds(0.1f);

            if (SCR_GameManager.gameManager.GetScene() != 3)
            {
                for (int i = 0; i < spawnedPlayers; i++)
                {
                    PlacePlayerOnSpawner(i);
                    players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player UI Controller");
                }
            }

            yield return new WaitForSeconds(2f);
            SCR_GameManager.gameManager.StartGame();
        }
        else
        {
            playerAmount = SCR_GameManager.gameManager.GetPlayerAmount();
            players = new GameObject[playerAmount];
            playerPrompt.gameObject.SetActive(true);
            for (int i = 3; i > playerAmount - 1; i--)
            {
                Destroy(spawners[i]);
            }
            SCR_GameManager.gameManager.StartCoroutine(SCR_GameManager.gameManager.LoadScene());
        }             
    }

    IEnumerator SetUp()
    {
        yield return new WaitForSeconds(0.1f);
        spawnedPlayers = 0;
    }

    public void ToggleJoining(bool join)
    {
        if(!join)
        {
            gameObject.GetComponent<PlayerInputManager>().DisableJoining();
        }
        else
        {
            gameObject.GetComponent<PlayerInputManager>().EnableJoining();
        }
    }
}
