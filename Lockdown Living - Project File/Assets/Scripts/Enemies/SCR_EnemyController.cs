using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCR_EnemyController : MonoBehaviour
{
    private SCR_PlayerManager pManager = SCR_PlayerManager.playerManager;

    [SerializeField] private float eLife = 100;

    private GameObject[] players;
    private float lookRadius = 10f;
    private List<int> playersInRadius = new List<int>();
    private bool chasingPlayer = false;
    private GameObject targetPlayer;

    private NavMeshAgent navMesh;
    private Animator anim;

    [SerializeField] private GameObject sword;
    private bool canAttack = false;
    private bool waitingToAttack = false;
    public GameObject hand;

    private SCR_EnemyManaager eManager;

    public void SetEManager(SCR_EnemyManaager script) => eManager = script;
    [SerializeField] GameObject deathParticles;

    void Start()
    {
        players = new GameObject[pManager.GetPlayerAmount()];
        for (int i = 0; i < pManager.GetPlayerAmount(); i++)
        {
            players[i] = pManager.GetPlayer(i);
        }
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (SCR_GameManager.gameManager.GetPlaying())
        {
            if (!chasingPlayer)
            {
                if (playersInRadius.Count > 0)
                {
                    playersInRadius.Clear();
                }

                for (int i = 0; i < pManager.GetPlayerAmount(); i++)
                {
                    float distance = Vector3.Distance(players[i].transform.position, transform.position);

                    if (distance <= lookRadius)
                    {
                        playersInRadius.Add(i);
                    }
                }

                if (playersInRadius.Count > 1)
                {
                    targetPlayer = players[Random.Range(0, playersInRadius.Count)];
                    chasingPlayer = true;
                }
                else if (playersInRadius.Count == 1)
                {
                    targetPlayer = players[playersInRadius[0]];
                    chasingPlayer = true;
                }
            }
            else
            {
                float distance = Vector3.Distance(targetPlayer.transform.position, transform.position);

                if (distance > lookRadius)
                {
                    chasingPlayer = false;
                    anim.SetBool("isMoving", false);
                }
                else
                {

                    if (distance <= navMesh.stoppingDistance)
                    {
                        FaceTarget();
                        anim.SetBool("isMoving", false);
                        if (canAttack)
                        {
                            StartCoroutine(Attack());
                        }
                        else if (!canAttack && !waitingToAttack)
                        {
                            StartCoroutine(WaitToAttack());
                        }
                    }
                    else
                    {
                        anim.SetBool("isMoving", true);
                        navMesh.SetDestination(targetPlayer.transform.position);
                    }
                }
            }
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        anim.SetTrigger("swing");
        yield return new WaitForSeconds(0.7f);
        sword.GetComponent<MeshCollider>().enabled = true;
        yield return new WaitForSeconds(1f);
        sword.GetComponent<MeshCollider>().enabled = false;
        sword.GetComponent<SCR_PickUps>().SetHitSomething(false);
        waitingToAttack = false;
    }

    IEnumerator WaitToAttack()
    {
        waitingToAttack = true;
        yield return new WaitForSeconds(2.5f);
        canAttack = true;
    }

    public void StopAll()
    {
        StopAllCoroutines();
        canAttack = false;
        sword.GetComponent<MeshCollider>().enabled = false;
        sword.GetComponent<SCR_PickUps>().SetHitSomething(false);
        waitingToAttack = false;
    }


    IEnumerator DestroyEnemy()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        Instantiate(deathParticles, gameObject.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        eManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    public void LoseLife(float damage, GameObject player)
    {
        eLife -= damage;
        if (eLife <= 0f)
        {
            player.GetComponent<SCR_PlayerData>().AddStar();
            StartCoroutine(DestroyEnemy());
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

}
