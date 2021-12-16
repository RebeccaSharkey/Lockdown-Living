using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.UI;
using UnityEngine.Audio;

public class SCR_GameManager : MonoBehaviour
{
    public static SCR_GameManager gameManager;

    private SCR_PlayerManager pManager;

    private bool playing = false;
    private bool paused = false;

    [SerializeField] private int numberOfPlayers = 1;

    [SerializeField] private GameObject twoPlayerPanel;
    [SerializeField] private GameObject threePlusPlayerPanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject endPanelButton;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text countDownText;
    [SerializeField] private Animator fadeAnim;

    public Camera mainCam;
    private int scene = 0;

    private bool loading = true;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countDown;

    #region Getters and Setters
    public int GetPlayerAmount() => numberOfPlayers;
    public void SetPlayerAmount(int players) => numberOfPlayers = players;

    public bool GetPlaying() => playing;
    public void SetPlaying(bool placeHolder) => playing = placeHolder;

    public GameObject GetTwoPlayerPanel() => twoPlayerPanel;
    public void SetTwoPlayerPanel(GameObject newObject) => twoPlayerPanel = newObject;

    public GameObject GetThreePlayerPanel() => threePlusPlayerPanel;
    public void SetThreePlayerPanel(GameObject newObject) => threePlusPlayerPanel = newObject;

    public Text GetText() => countDownText;
    public void SetText(Text newObject) => countDownText = newObject;

    public int GetScene() => scene;
    public void SetScene(int newScene) => scene = newScene;

    public Animator GetAnimator() => fadeAnim;
    public void SetAnimator(Animator anim) => fadeAnim = anim;

    public GameObject GetStartPanel() => startPanel;

    public GameObject GetEndPanel() => endPanel;
    #endregion

    private void Awake()
    {
        if (gameManager)
        {
            gameManager.StopAllCoroutines();
            gameManager.SetScene(SceneManager.GetActiveScene().buildIndex);
            gameManager.NewScene();
            Destroy(gameObject);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        pManager = SCR_PlayerManager.playerManager;
        NewScene();
    }

    public void Pause(bool endOfScene, int playerPause)
    {
        playing = false;

        if (endOfScene)
        {
            switch (scene)
            {
                case 1:
                    twoPlayerPanel.GetComponentInParent<SCR_Timer>().SetTimer(900f);
                    for (int i = 0; i < numberOfPlayers; i++)
                    {
                        pManager.GetPlayer(i).GetComponent<SCR_PlayerController>().StopAll();
                    }
                    StartCoroutine(ChangeScene("Made Up Scene"));
                    break;
                case 2:
                    twoPlayerPanel.GetComponentInParent<SCR_Timer>().SetTimer(1200f);
                    for (int i = 0; i < numberOfPlayers; i++)
                    {
                        pManager.GetPlayer(i).GetComponent<SCR_PlayerController>().StopAll();
                        pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().StopAllCoroutines();
                    }
                    StartCoroutine(ChangeScene("Clap For NHS"));
                    break;
                case 4:
                    StartCoroutine(StopGame());
                    break;
            }
        }
        else
        {
            Time.timeScale = 0;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().StopAllCoroutines();
                pManager.GetPlayer(i).GetComponent<PlayerInput>().SwitchCurrentActionMap("Player UI Controller");
                if (i == playerPause)
                {
                    pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetPauseMenuPanel().SetActive(true);
                }
                else
                {
                    pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetPausedPanel().SetActive(true);
                    pManager.GetPlayer(i).GetComponent<PlayerInput>().DeactivateInput();
                }
                pManager.GetPlayer(i).GetComponent<PlayerInput>().gameObject.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(null);
                pManager.GetPlayer(i).GetComponent<PlayerInput>().gameObject.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetResumeButton());
            }

            paused = true;
        }
    }

    public void SetUp_()
    {
        if (scene != 3)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                pManager.GetPlayer(i).GetComponent<PlayerInput>().DeactivateInput();
                pManager.GetPlayer(i).GetComponent<PlayerInput>().SwitchCurrentActionMap("Player UI Controller");
                pManager.GetPlayer(i).GetComponent<PlayerInput>().DeactivateInput();
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetMainPanel().SetActive(false);
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetReadyPanel().SetActive(true);
                pManager.GetPlayer(i).GetComponent<PlayerInput>().gameObject.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(null);
                pManager.GetPlayer(i).GetComponent<PlayerInput>().gameObject.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetReadyButton());
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetReadyPanel().GetComponentInParent<SCR_PlayerUIControls>().GetReadyText().SetActive(false);
            }

            mainCam.enabled = false;

            countDownText.gameObject.SetActive(true);
            countDownText.text = "Ready Up!";
            if (loading)
            {
                loading = false;
                StartCoroutine(LoadScene());
            }
        }
        else
        {
            countDownText.gameObject.SetActive(true);
            countDownText.text = "Ready Up!";
            if (loading)
            {
                loading = false;
                StartCoroutine(LoadScene());
            }
        }
    }

    public void CheckAllReady()
    {
        scene = scene = SceneManager.GetActiveScene().buildIndex;

        StartCoroutine(WaitOnCheck());
    }

    IEnumerator Setup()
    {

        infoPanel.GetComponent<SCR_Info>().Deactivate();
        infoPanel.SetActive(true);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            pManager.GetPlayer(i).GetComponent<PlayerInput>().SwitchCurrentActionMap("Player Movement");
            pManager.GetPlayer(i).GetComponent<PlayerInput>().DeactivateInput();
            pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetReadyPanel().SetActive(false);
        }

        countDownText.gameObject.SetActive(true);

        countDownText.gameObject.GetComponent<Animator>().Play("CountDown");
        audioSource.clip = countDown;
        countDownText.text = "3";
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        countDownText.text = "GO!";
        audioSource.Play();
        yield return new WaitForSeconds(1f);

        countDownText.gameObject.SetActive(false);

        if (scene != 3)
        {
            twoPlayerPanel.GetComponentInParent<SCR_Timer>().ToggleTimer(true);
        }

        for (int i = 0; i < numberOfPlayers; i++)
        {
            pManager.GetPlayer(i).GetComponent<PlayerInput>().ActivateInput();
            pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().BeginGame(scene);
        }

        playing = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        scene = scene = SceneManager.GetActiveScene().buildIndex;
        if (numberOfPlayers == 2)
        {
            twoPlayerPanel.SetActive(true);
        }
        else if (numberOfPlayers > 2)
        {
            threePlusPlayerPanel.SetActive(true);

        }
        if (scene == 2)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
            foreach (GameObject gObject in objects)
            {
                gObject.GetComponent<SphereCollider>().enabled = true;
            }
        }
        twoPlayerPanel.GetComponentInParent<SCR_Timer>().ToggleTimer(false);

        if (!paused)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                pManager.GetPlayer(i).GetComponentInChildren<Camera>().enabled = true;
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().SetIsReady(false);
            }
            if(scene != 3)
            {
                SetUp_();
            }

        }
        else
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetPauseMenuPanel().SetActive(false);
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetPausedPanel().SetActive(false);
                pManager.GetPlayer(i).GetComponent<PlayerInput>().SwitchCurrentActionMap("Player Movement");
                pManager.GetPlayer(i).GetComponent<PlayerInput>().ActivateInput();
                pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().BeginGame(scene);
            }
            twoPlayerPanel.GetComponentInParent<SCR_Timer>().ToggleTimer(true);
            playing = true;
            paused = false;
        }
    }

    public void NewScene()
    {
        mainCam = Camera.main;
        countDownText.enabled = true;
    }

    IEnumerator WaitOnCheck()
    {
        bool ready = true;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (!pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetIsReady())
            {
                ready = false;
            }
        }

        yield return new WaitForSeconds(1f);

        if (ready)
        {
            if (scene != 3)
            {
                StartCoroutine(Setup());
            }
            else
            {
                StartCoroutine(ChangeScene("Out Side"));
            }
        }
    }

    public IEnumerator LoadScene()
    {
        if(scene != 3)
        {
            infoPanel.SetActive(true);
            infoPanel.GetComponent<SCR_Info>().SetLevelInfo(scene);
        }
        yield return new WaitForSeconds(5f);
        fadeAnim.SetTrigger("Start");
        if (scene == 3)
        {
            pManager.ToggleJoining(true);
        }
        else
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                pManager.GetPlayer(i).GetComponent<PlayerInput>().ActivateInput();
            }
        }
    }

    public IEnumerator ChangeScene(string newScene)
    {
        playing = false;
        fadeAnim.SetTrigger("End");
        loading = true;
        scene = scene = SceneManager.GetActiveScene().buildIndex;
        if (scene != 0)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (pManager.GetPlayer(i).GetComponent<SCR_PlayerController>().item != null)
                {
                    pManager.GetPlayer(i).GetComponent<SCR_PlayerController>().StartCoroutine(pManager.GetPlayer(i).GetComponent<SCR_PlayerController>().Throw());
                }
            }

            yield return new WaitForSeconds(2f);

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");
            foreach (GameObject gObject in objects)
            {
                Destroy(gObject);
            }
        }

        yield return new WaitForSeconds(2f);


        SceneManager.LoadScene(newScene);
    }

    IEnumerator StopGame()
    {
        countDownText.gameObject.SetActive(true);
        countDownText.text = "Stop!";
        for (int i = 0; i < numberOfPlayers; i++)
        {
            pManager.GetPlayer(i).GetComponent<PlayerInput>().DeactivateInput();
        }
        yield return new WaitForSeconds(1f);
        mainCam.enabled = true;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            pManager.GetPlayer(i).GetComponent<SCR_PlayerController>().StopAll();
            pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().StopAllCoroutines();
            pManager.GetPlayer(i).GetComponentInChildren<Camera>().enabled = false;
            pManager.GetPlayer(i).GetComponent<PlayerInput>().SwitchCurrentActionMap("Player UI Controller");
            pManager.GetPlayer(i).GetComponent<PlayerInput>().ActivateInput();
        }
        endPanel.GetComponent<SCR_End>().SetUpEnd();
        endPanel.SetActive(true);
        SCR_Event.eventSystem.gameObject.GetComponent<MultiplayerEventSystem>().playerRoot = pManager.GetPlayer(0);
        SCR_Event.eventSystem.gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(null);
        SCR_Event.eventSystem.gameObject.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(endPanelButton);
    }
}
