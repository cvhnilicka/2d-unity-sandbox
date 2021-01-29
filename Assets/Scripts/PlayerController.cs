using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    // config
    [Header("Config")]
    [SerializeField] float runSpeed = 3.0f;
    [SerializeField] float jumpSpeed = 5.0f;


    // state
    bool isAlive = true;


    // cached componenets
    [Header("Components")]
    Rigidbody2D myBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;


    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        RunAnimation();
        Jump();
        Death();
        //JumpAnimation();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myBody.velocity.y);
        myBody.velocity = playerVelocity;
    }

    private void Jump()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("foreground")))
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
                myBody.velocity += jumpVelocity;

            }
        }
        
    }

    private void FlipSprite()
    {
       bool playerHasXVelocity = Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon;
       if (playerHasXVelocity)
        {
            transform.localScale = new Vector2(Mathf.Sign(myBody.velocity.x), 1f);
        }
    }

    private void JumpAnimation()
    {
        if (Mathf.Abs(myBody.velocity.y) > 0)
        {
            myAnimator.SetBool("Jumping", true);
        } else
        {
            myAnimator.SetBool("Jumping", false);
        }
    }

    private void RunAnimation()
    {
        if (Mathf.Abs(myBody.velocity.x) > Mathf.Epsilon)
        {
            myAnimator.SetBool("Running", true);
        } else
        {
            myAnimator.SetBool("Running", false);
        }
    }

    private void Death()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("enemy")))
        {
            // here i could trigger death anim
            //myAnimator.SetTrigger("die");


            Debug.Log("Youre deab bro");
            isAlive = false;
            // will need to stop movement/velocity
        }
    }
}
