using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_Tutorial : MonoBehaviour
{

    private CharacterController controller;

    private bool grounded;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private Vector3 playerVelocity;
    private Vector2 playerMovement;

    private bool jump;
    private bool canJump = true;

    public bool carryingItem = false;
    public GameObject item = null;

    private bool canThrow = true;
    [SerializeField] private GameObject throwFrom;
    [SerializeField] private float force = 10f;

    private int punch = 0;
    [SerializeField] public GameObject leftHand;
    [SerializeField] public GameObject rightHand;
    private bool canPunch = true;

    private Animator anim;

    public int hits = 0;

    public void Movement(InputAction.CallbackContext callbackContext)
    {
        playerMovement = callbackContext.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        jump = callbackContext.action.triggered;

    }

    public void DropItem(InputAction.CallbackContext callbackContext)
    {
        if (carryingItem && canThrow)
        {
            StartCoroutine(Throw());
        }
    }

    public void Punch(InputAction.CallbackContext callbackContext)
    {

        if (callbackContext.started && gameObject.scene.IsValid() && canPunch)
        {
            canPunch = false;
            if (!carryingItem)
            {
                if (punch == 0)
                {
                    punch++;
                    StartCoroutine(Punch("punch"));
                    rightHand.GetComponent<SphereCollider>().enabled = true;
                }
                else if (punch == 1)
                {
                    punch--;
                    StartCoroutine(Punch("punch_2"));
                    leftHand.GetComponent<SphereCollider>().enabled = true;
                }
            }
            else
            {

                if (!item.GetComponent<SCR_TutorialObject>().bigItem)
                {
                    StartCoroutine(Punch("punch"));
                }
                else
                {
                    StartCoroutine(Punch("swing"));
                }
            }
        }
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

        anim = gameObject.GetComponentInChildren<Animator>();

        rightHand.GetComponent<SphereCollider>().enabled = false;
        leftHand.GetComponent<SphereCollider>().enabled = false;
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        anim.SetBool("isMoving", false);

        grounded = controller.isGrounded;
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(playerMovement.x, 0, playerMovement.y);

        if (move != Vector3.zero)
        {
            if ((grounded && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) || (grounded && anim.GetCurrentAnimatorStateInfo(0).IsName("Running")))
            {
                gameObject.transform.forward = move;
                controller.Move(move * Time.deltaTime * speed);
                anim.SetBool("isMoving", true);
            }
            else if (!grounded)
            {
                gameObject.transform.forward = move;
                controller.Move(move * Time.deltaTime * speed);
            }
        }

        if (jump && grounded && canJump)
        {
            canJump = false;
            StartCoroutine(Jump());
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator Jump()
    {
        jump = false;
        anim.SetTrigger("jumped");
        yield return new WaitForSeconds(1.1f);
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

    public IEnumerator Throw()
    {
        anim.SetTrigger("throw");
        carryingItem = false;
        yield return new WaitForSeconds(0.8f);
        item.transform.position = throwFrom.transform.position;
        item.transform.parent = null;
        Rigidbody itemRB = item.GetComponent<Rigidbody>();
        itemRB.useGravity = true;
        itemRB.constraints = RigidbodyConstraints.None;
        itemRB.AddForce(transform.forward * force);
        item.GetComponent<SCR_TutorialObject>().beenThrown = true;
        item.GetComponent<MeshCollider>().enabled = true;
        item = null;
    }

    IEnumerator Punch(string animation)
    {
        anim.SetTrigger(animation);
        yield return new WaitForSeconds(0.5f);
        if (item != null)
        {
            item.GetComponent<MeshCollider>().enabled = true;
        }

        yield return new WaitForSeconds(1f);

        rightHand.GetComponent<SphereCollider>().enabled = false;
        leftHand.GetComponent<SphereCollider>().enabled = false;
        if (item != null)
        {
            item.GetComponent<MeshCollider>().enabled = false;
            item.GetComponent<SCR_TutorialObject>().hitSomething = false;
        }

        yield return new WaitForSeconds(1f);
        canPunch = true;
    }

}
