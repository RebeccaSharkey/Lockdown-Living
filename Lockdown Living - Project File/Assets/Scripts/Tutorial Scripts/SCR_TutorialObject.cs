using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TutorialObject : MonoBehaviour
{

    [SerializeField] private Vector3 offSet;
    [SerializeField] private Quaternion rotationOffset;
    public bool beenThrown;
    public bool hitSomething;
    public GameObject currentPlayer;
    public bool bigItem = false;
    [SerializeField] private GameObject door;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.gameObject.transform.root.gameObject;
            if (!currentPlayer.GetComponent<SCR_Tutorial>().carryingItem)
            {
                beenThrown = false;
                currentPlayer.GetComponent<SCR_Tutorial>().carryingItem = true;
                currentPlayer.GetComponent<SCR_Tutorial>().item = gameObject;
                gameObject.transform.parent = currentPlayer.GetComponent<SCR_Tutorial>().rightHand.transform;
                transform.localRotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
                transform.localPosition = offSet;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                gameObject.GetComponent<MeshCollider>().enabled = false;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                hitSomething = false;
                Destroy(door);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !hitSomething)
        {
            hitSomething = true;
            collision.gameObject.GetComponent<SCR_TutorialEnemy>().Hit();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            if (beenThrown == true)
            {
                beenThrown = false;
                gameObject.GetComponent<SphereCollider>().enabled = true;
                currentPlayer = null;
            }
        }
    }
}
