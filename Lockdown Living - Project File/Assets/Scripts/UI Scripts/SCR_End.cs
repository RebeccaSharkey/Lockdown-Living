using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SCR_End : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winningPlayerText;
    [SerializeField] private TextMeshProUGUI[] playerScores = new TextMeshProUGUI[4];

    [SerializeField] private SCR_GameManager gManager;
    [SerializeField] private SCR_PlayerManager pManager;

    private bool isTie = true;

    private int[] scores;

    public void SetUpEnd()
    {
        int amountOfPlayers = gManager.GetPlayerAmount();
        scores = new int[amountOfPlayers];

        for (int i = 0; i < 4; i++)
        {
            if(i < gManager.GetPlayerAmount())
            {
                playerScores[i].text = pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetStars().ToString();
                scores[i] = pManager.GetPlayer(i).GetComponent<SCR_PlayerData>().GetStars();
            }
            else
            {
                playerScores[i].text = "Null";
            }
        }


        int highestScore = 0;
        for (int i = 0; i < amountOfPlayers; i++)
        {
            if(scores[i] > highestScore)
            {
                highestScore = scores[i];
                isTie = false;
            }
            else if (scores[i] == highestScore)
            {
                isTie = true;
            }
        }

        if(!isTie)
        {
            winningPlayerText.text = "Player " + (scores.ToList().IndexOf(highestScore) + 1) + " wins!";
        }
        else
        {
            winningPlayerText.text = "Its a tie, No one wins!";
        }
    }

    public void OnMainMenu()
    {
        SCR_MainMenu.mainMenu.OnMainMenu(transform.root.gameObject);
    }


    public void OnQuit()
    {
        Application.Quit();
    }
}
