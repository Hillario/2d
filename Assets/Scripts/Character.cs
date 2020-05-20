using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float movespeed=10f;   
    private Rigidbody2D myRigidbody;
    private SpriteRenderer mySpriteRenderer;
    private bool idletorun;

    //animation variables   
    private Animator anim;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float groundRadius=0.2f;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float jumpForce=30f;

    [SerializeField]
    private Transform bulletPoint;

    [SerializeField]
    private GameObject myBullet;

   
    
    private bool isgrounded;
    private bool doubleJump;
 

    // Start is called before the first frame update
    void Start()
    {
        //access components
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();        

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {

        handleInputs();
        
        if (isgrounded)
        {
            doubleJump = false;
            
        }
        

        isgrounded = isGrounded();
        handleMovement();

        //instantiate bullet
        Bullet();

        //accelerate the bullet
        




    }

    private void handleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        myRigidbody.velocity = new Vector2(horizontal * movespeed, myRigidbody.velocity.y);

        //implementing the flip algorithm
        //setting the idletorun parameter
        if (horizontal > 0)
        {
            mySpriteRenderer.flipX = false;
            idletorun = true;
        }
        else if (horizontal < 0)
        {
            mySpriteRenderer.flipX = true;
            idletorun = true;
        }else if (horizontal == 0)
        {
            idletorun = false;
        }

        anim.SetBool("idletorun", idletorun);

    }

    private void handleInputs()
    {
        if ((isgrounded || !doubleJump) && Input.GetButton("Jump"))//OnTouch()--ForAndroid
        {
            anim.SetBool("grounded", isgrounded);
            myRigidbody.AddForce(new Vector2(0f, jumpForce));
            if (!doubleJump && !isgrounded)
            {
                doubleJump = true;
            }
        }
        else
        {
            anim.SetBool("grounded", isgrounded);
        }
    }

    private bool isGrounded()
    {
        if (myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                //contains all the collliders coliding with the player
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    //if colliders in player is not equivalent to attached, then it's grounded
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void Bullet()
    {
        if (Input.GetButton("Fire1"))
        {
            Instantiate(myBullet, bulletPoint.position, Quaternion.identity);            
        }
    }

}
