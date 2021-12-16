using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Info : MonoBehaviour
{
    [SerializeField] private GameObject LevelOne;
    [SerializeField] private GameObject LevelTwo;
    [SerializeField] private GameObject LevelThree;

    public void SetLevelInfo(int i)
    {
        LevelOne.SetActive(true);
        LevelTwo.SetActive(true);
        LevelThree.SetActive(true);

        switch (i)
        {
            case 1:
                LevelOne.SetActive(true);
                LevelTwo.SetActive(false);
                LevelThree.SetActive(false);
                break;
            case 2:
                LevelOne.SetActive(false);
                LevelTwo.SetActive(true);
                LevelThree.SetActive(false);
                break;
            case 4:
                LevelOne.SetActive(false);
                LevelTwo.SetActive(false);
                LevelThree.SetActive(true);
                break;
        }
    }

    public void Deactivate()
    {
        LevelOne.SetActive(false);
        LevelTwo.SetActive(false);
        LevelThree.SetActive(false);
    }

}
