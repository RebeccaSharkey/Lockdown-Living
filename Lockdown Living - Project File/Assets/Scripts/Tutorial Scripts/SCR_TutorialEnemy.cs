using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TutorialEnemy : MonoBehaviour
{

    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject door;

    public void Hit()
    {
        anim.SetTrigger("hit");
        Destroy(door);
        StartCoroutine(DestroyEnemy());
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.2f);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        Instantiate(deathParticles, gameObject.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

}
