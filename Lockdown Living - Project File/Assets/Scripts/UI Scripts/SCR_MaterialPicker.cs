using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MaterialPicker : MonoBehaviour
{
    [SerializeField] private Material[] playerColours;
    [SerializeField] private GameObject[] playerSpawner;
    private bool[] colourInUse;

    public void SetUp()
    {
        colourInUse = new bool[playerColours.Length];
        playerSpawner = new GameObject[SCR_GameManager.gameManager.GetPlayerAmount()];
        for (int i = 0; i < playerColours.Length; i++)
        {
            if (i < SCR_GameManager.gameManager.GetPlayerAmount())
            {
                colourInUse[i] = true;
                playerSpawner[i] = SCR_PlayerManager.playerManager.GetSpawner(i);
            }
            else
            {
                colourInUse[i] = false;
            }
        }
    }

    public void ChangeColourRight(int playerIndex)
    {
        int currentMaterial = -1;
        int placeHolderMaterial = currentMaterial;
        for (int i = 0; i < playerColours.Length; i++)
        {
            if(playerColours[i] == SCR_PlayerManager.playerManager.GetPlayer(playerIndex).GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials[0])
            {
                currentMaterial = i;
                placeHolderMaterial = currentMaterial;
            }
        }

        bool newColourFound = false;
        while (!newColourFound)
        {
            currentMaterial++;
            if(currentMaterial >= playerColours.Length)
            {
                currentMaterial = 0;
            }

            if(!colourInUse[currentMaterial])
            {
                colourInUse[currentMaterial] = true;
                colourInUse[placeHolderMaterial] = false;
                playerSpawner[playerIndex].GetComponent<MeshRenderer>().sharedMaterial = playerColours[currentMaterial];
                SCR_PlayerManager.playerManager.GetPlayer(playerIndex).GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = playerColours[currentMaterial];
                newColourFound = true;
            }
        }
    }

    public void ChangeColourLeft(int playerIndex)
    {
        int currentMaterial = -1;
        int placeHolderMaterial = currentMaterial;
        for (int i = 0; i < playerColours.Length; i++)
        {
            if (playerColours[i] == SCR_PlayerManager.playerManager.GetPlayer(playerIndex).GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials[0])
            {
                currentMaterial = i; 
                placeHolderMaterial = currentMaterial;
            }
        }

        bool newColourFound = false;
        while (!newColourFound)
        {
            currentMaterial--;
            if (currentMaterial < 0)
            {
                currentMaterial = playerColours.Length - 1;
            }

            if (!colourInUse[currentMaterial])
            {
                colourInUse[currentMaterial] = true;
                colourInUse[placeHolderMaterial] = false;
                playerSpawner[playerIndex].GetComponent<MeshRenderer>().material = playerColours[currentMaterial];
                SCR_PlayerManager.playerManager.GetPlayer(playerIndex).GetComponentInChildren<SkinnedMeshRenderer>().material = playerColours[currentMaterial];
                newColourFound = true;
            }
        }
    }
}
