using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        SCR_GameManager.gameManager.StartGame();
    }

    public void MainMenu()
    {
        SCR_MainMenu.mainMenu.OnMainMenu(transform.root.gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
