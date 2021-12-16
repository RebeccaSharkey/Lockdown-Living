using System.Collections;
using UnityEngine;

public class SCR_PickUps : MonoBehaviour
{
    [SerializeField] private float oDamage = 0;
    [SerializeField] private float oLife = 1;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private Quaternion rotationOffset;

    public GameObject currentPlayer = null;
    private bool hitSomething = true;
    private bool beenThrown = false;

    [SerializeField] private bool bigItem = false;

    [SerializeField] GameObject deathParticles;


    public void SetBeenThrown(bool placeHolder) => beenThrown = placeHolder;

    public void SetHitSomething(bool placeHolder) => hitSomething = placeHolder;

    public float GetDamage() => oDamage;

    public bool GetBigItem() => bigItem;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            currentPlayer = other.transform.root.gameObject;
            SCR_PlayerController playerController = currentPlayer.GetComponent<SCR_PlayerController>();
            if(playerController != null)
            {
                if (!playerController.GetCarryingItemBool())
                {
                    beenThrown = false;
                    transform.parent = playerController.GetRightHand().transform;
                    transform.localRotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
                    transform.localPosition = offSet;
                    playerController.SetItem(gameObject);
                    playerController.SetCarryingItemBool(true);
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    gameObject.GetComponent<MeshCollider>().enabled = false;
                    hitSomething = false;
                }
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            currentPlayer = other.transform.root.gameObject;
            transform.parent = currentPlayer.GetComponent<SCR_EnemyController>().hand.transform;
            transform.localRotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
            transform.localPosition = offSet;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.GetComponent<MeshCollider>().enabled = false;
            hitSomething = false;
        }
    }

    IEnumerator Destroy()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        Instantiate(deathParticles, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

    private void CheckAndDestroy()
    {
        if(oLife == 0)
        {
            currentPlayer.GetComponent<SCR_PlayerController>().SetCarryingItemBool(false);
            StartCoroutine(Destroy());
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if (beenThrown == true)
            {
                beenThrown = false;
                gameObject.GetComponent<SphereCollider>().enabled = true;
                currentPlayer = null;
                oLife--;
                CheckAndDestroy();
            }
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            if(collision.transform.root.gameObject != currentPlayer)
            {
                if(!hitSomething)
                {
                    hitSomething = true;
                    Animator anim = collision.transform.root.gameObject.GetComponent<SCR_PlayerController>().GetAnim();
                    collision.transform.root.gameObject.GetComponent<SCR_PlayerController>().StopAll();
                    anim.SetTrigger("hit");
                    if(currentPlayer.CompareTag("Player"))
                    {
                        if (currentPlayer.GetComponent<SCR_PlayerData>().GetScene() == 1)
                        {
                            transform.root.gameObject.GetComponent<SCR_PlayerData>().AddStar();
                        }
                        else
                        {
                            currentPlayer.GetComponent<SCR_PlayerData>().AddHappiness(10f + oDamage);
                            oLife--;
                            CheckAndDestroy();
                        }
                    }
                    else
                    {
                        collision.transform.root.gameObject.GetComponent<SCR_PlayerData>().RemoveStar();
                    }                                      
                }
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.transform.root.gameObject != currentPlayer && !currentPlayer.CompareTag("Enemy"))
            {
                if (!hitSomething)
                {
                    hitSomething = true;
                    Animator anim = collision.transform.root.gameObject.GetComponentInChildren<Animator>();
                    anim.SetTrigger("hit");
                    collision.transform.root.gameObject.GetComponent<SCR_EnemyController>().StopAll();
                    collision.transform.root.gameObject.GetComponent<SCR_EnemyController>().LoseLife(100f, currentPlayer);
                }
            }                
        }
    }

}
