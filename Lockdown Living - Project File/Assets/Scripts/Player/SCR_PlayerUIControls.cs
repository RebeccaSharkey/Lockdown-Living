using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_PlayerUIControls : MonoBehaviour
{
    [SerializeField] private GameObject readyText;
    [SerializeField] private GameObject readyButton;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;

    public GameObject GetReadyText() => readyText;

    IEnumerator WaitForReady()
    {
        readyText.SetActive(true);
        yield return new WaitForSeconds(3f);
        GetComponentInParent<SCR_PlayerData>().SetIsReady(true);
        SCR_GameManager.gameManager.CheckAllReady();
    }

    public void OnReady()
    {
        GetComponentInParent<SCR_PlayerController>().GetAnim().SetTrigger("Spin");
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        StartCoroutine(WaitForReady());
    }

    public void OnLeft()
    {
        SCR_GameManager.gameManager.gameObject.GetComponent<SCR_MaterialPicker>().ChangeColourLeft(SCR_PlayerManager.playerManager.GetPlayerIndex(transform.parent.gameObject));
    }

    public void OnRight()
    {
        SCR_GameManager.gameManager.gameObject.GetComponent<SCR_MaterialPicker>().ChangeColourRight(SCR_PlayerManager.playerManager.GetPlayerIndex(transform.parent.gameObject));
    }
}
