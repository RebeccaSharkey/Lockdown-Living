using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_PlayerController : MonoBehaviour
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

    private bool carryingItem = false;
    public GameObject item = null;

    private bool canThrow = true;
    [SerializeField] private GameObject throwFrom;
    [SerializeField] private float force = 10f;

    private int punch = 0;
    private bool isPunching = false;
    private bool hitSomething;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    private bool canPunch = true;

    private Animator anim;

    public int hits = 0;

    private bool canClap = true;

    private bool canPause = true;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clap;


    #region Getters and Setters
    public bool GetCarryingItemBool() => carryingItem;
    public void SetCarryingItemBool(bool placeholder) => carryingItem = placeholder;

    public GameObject GetItem() => item;
    public void SetItem(GameObject placeholder) => item = placeholder;

    public GameObject GetLeftHand() => leftHand;
    public GameObject GetRightHand() => rightHand;

    public GameObject GetThrowFrom() => throwFrom;

    public Animator GetAnim() => anim;

    public bool GetIsPunching() => isPunching;

    public void SetHitSomething(bool input) => hitSomething = input;
    public bool GetHitSomething() => hitSomething;

    public void SetCanThrow(bool placeHolder) => canThrow = placeHolder;
    #endregion

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

        anim = gameObject.GetComponentInChildren<Animator>();

        rightHand.GetComponent<SphereCollider>().enabled = false;
        leftHand.GetComponent<SphereCollider>().enabled = false;
    }

    public void Movement(InputAction.CallbackContext callbackContext)
    {
        if (SCR_GameManager.gameManager.GetScene() == 1 || SCR_GameManager.gameManager.GetScene() == 2)
        {
            playerMovement = callbackContext.ReadValue<Vector2>();
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (SCR_GameManager.gameManager.GetScene() == 1 || SCR_GameManager.gameManager.GetScene() == 2)
        {
            jump = callbackContext.action.triggered;
        }
        else if (SCR_GameManager.gameManager.GetScene() == 4 && canClap)
        {
            StartCoroutine(Clap());
            gameObject.GetComponent<SCR_PlayerData>().AddStar();
        }

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
        if (SCR_GameManager.gameManager.GetScene() == 1 || SCR_GameManager.gameManager.GetScene() == 2)
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

                    if (!item.GetComponent<SCR_PickUps>().GetBigItem())
                    {
                        punch = 0;
                        StartCoroutine(Punch("punch"));
                        item.GetComponent<MeshCollider>().enabled = true;
                    }
                    else
                    {
                        StartCoroutine(Punch("swing"));
                    }
                }
            }
        }        
    }

    public void Pause(InputAction.CallbackContext callbackContext)
    {
        if(SCR_GameManager.gameManager.GetScene() != 0 || SCR_GameManager.gameManager.GetScene() != 3)
        {
            if (canPause)
            {
                StartCoroutine(Pause());
            }
        }
    }

    void FixedUpdate()
    {
        if(SCR_GameManager.gameManager.GetScene() == 1 || SCR_GameManager.gameManager.GetScene() == 2)
        {
            PlayerMovement();
        }
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
        item.GetComponent<SCR_PickUps>().SetBeenThrown(true);
        item.GetComponent<MeshCollider>().enabled = true;
        item = null;
    }

    IEnumerator Punch(string animation)
    {
        anim.SetTrigger(animation);
        isPunching = true;
        yield return new WaitForSeconds(0.5f);
        if (item != null)
        {
            item.GetComponent<MeshCollider>().enabled = true;
        }

        yield return new WaitForSeconds(1f);

        isPunching = false;
        hitSomething = false;
        rightHand.GetComponent<SphereCollider>().enabled = false;
        leftHand.GetComponent<SphereCollider>().enabled = false;
        if (item != null)
        {
            item.GetComponent<MeshCollider>().enabled = false;
            item.GetComponent<SCR_PickUps>().SetHitSomething(false);
        }

        yield return new WaitForSeconds(1f);
        canPunch = true;
    }

    IEnumerator Clap()
    {
        canClap = false;
        anim.SetTrigger("Clap");
        audioSource.clip = clap;
        audioSource.Play();
        yield return new WaitForSeconds(0.17f);
        canClap = true;
    }

    IEnumerator Pause()
    {
        canPause = false;
        SCR_GameManager.gameManager.Pause(false, SCR_PlayerManager.playerManager.GetPlayerIndex(gameObject));
        yield return new WaitForSeconds(0.1f);
        canPause = true;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void StopAll()
    {
        StopAllCoroutines();
        isPunching = false;
        hitSomething = false;
        rightHand.GetComponent<SphereCollider>().enabled = false;
        leftHand.GetComponent<SphereCollider>().enabled = false;

        if (item != null)
        {
            item.GetComponent<MeshCollider>().enabled = false;
            item.GetComponent<SCR_PickUps>().SetHitSomething(false);
        }

        if(!carryingItem && item != null)
        {
            Rigidbody itemRB = item.GetComponent<Rigidbody>();
            itemRB.useGravity = true;
            itemRB.constraints = RigidbodyConstraints.None;
            item.GetComponent<SCR_PickUps>().SetBeenThrown(true);
            item.GetComponent<MeshCollider>().enabled = true;
            item = null;
        }

        StartCoroutine(Wait(1f));
        canPunch = true;
        canJump = true;
        canPause = true;
    }
}
