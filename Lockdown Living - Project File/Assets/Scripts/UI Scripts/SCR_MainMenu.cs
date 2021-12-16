using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MainMenu : MonoBehaviour
{
    public static SCR_MainMenu mainMenu;

    private void Awake()
    {
        if (mainMenu)
        {
            Destroy(gameObject);
        }
        else
        {
            mainMenu = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnMainMenu(GameObject player)
    {
        StartCoroutine(ChangeToMain(player));
    }

    IEnumerator ChangeToMain(GameObject player)
    {
        Time.timeScale = 1;
        GameObject[] spawners;
        spawners = GameObject.FindGameObjectsWithTag("Spawners");
        foreach (GameObject spawner in spawners)
        {
            Destroy(spawner);
        }

        GameObject[] enemys;
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }

        SCR_GameManager.gameManager.mainCam.enabled = true;

        Destroy(SCR_PlayerManager.playerManager.gameObject);
        Destroy(SCR_Canvas.canvas.gameObject);
        Destroy(SCR_GameManager.gameManager.gameObject);
        Destroy(SCR_Event.eventSystem.gameObject);

        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gPlayer in players)
        {
            if(gPlayer != player)
            {
                Destroy(gPlayer);
            }
        }

        yield return new WaitForSeconds(0.5f);

        Destroy(player);
        SceneManager.LoadScene("Start Menu");
    }
}
