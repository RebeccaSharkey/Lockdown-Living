using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PunchHit : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<SCR_TutorialEnemy>().Hit();
        }
    }
}