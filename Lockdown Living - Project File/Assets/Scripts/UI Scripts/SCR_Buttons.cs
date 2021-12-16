using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class SCR_Buttons : MonoBehaviour
{
    [SerializeField] private MultiplayerEventSystem eventSystem;

    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject startMenuStartButton;

    [SerializeField] private GameObject playerSelection;
    [SerializeField] private GameObject playerSelectionStartButton;

    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject controlsPanelStartButton;

    public void OnPlay()
    {
        startMenuPanel.SetActive(false);
        playerSelection.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(playerSelectionStartButton);
    }

    public void OnBack()
    {
        playerSelection.SetActive(false);
        controlsPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(startMenuStartButton);
    }

    public void OnControls()
    {
        startMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(controlsPanelStartButton);
    }

    public void OnTutorial()
    {
        Destroy(SCR_PlayerManager.playerManager.gameObject);
        Destroy(SCR_Canvas.canvas.gameObject);
        Destroy(SCR_GameManager.gameManager.gameObject);
        Destroy(SCR_Event.eventSystem.gameObject);
        SceneManager.LoadScene("Tutorial");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnPlayerAmountChoice()
    {
        string text = eventSystem.currentSelectedGameObject.name;
        if (text == "2")
        {
            SCR_GameManager.gameManager.SetPlayerAmount(2);
        }
        else if (text == "3")
        {
            SCR_GameManager.gameManager.SetPlayerAmount(3);
        }
        else if (text == "4")
        {
            SCR_GameManager.gameManager.SetPlayerAmount(4);
        }
        SCR_GameManager.gameManager.GetStartPanel().SetActive(false);
        SCR_GameManager.gameManager.StartCoroutine(SCR_GameManager.gameManager.ChangeScene("PlayerScreen"));
    }
}
