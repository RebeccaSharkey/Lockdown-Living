using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Hit : MonoBehaviour
{
    private SCR_PlayerController playerController;

    private void Awake()
    {
        playerController = gameObject.GetComponentInParent<SCR_PlayerController>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.root != transform.root)
        {
            if (!playerController.GetHitSomething() && playerController.GetIsPunching())
            {
                playerController.SetHitSomething(true);
                Animator anim = collision.transform.root.gameObject.GetComponent<SCR_PlayerController>().GetAnim();
                collision.transform.root.gameObject.GetComponent<SCR_PlayerController>().StopAll();
                anim.SetTrigger("hit");
                gameObject.GetComponentInParent<SCR_PlayerData>().AddHappiness(10f);
            }
        }
    }
}
