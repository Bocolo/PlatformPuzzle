using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController : MonoBehaviour
    {

   
        public Vector2 move;
        Rigidbody2D rb;
        bool isJumping;
        bool canJump;

        [SerializeField] bool isOnSurface;
        [SerializeField] bool isGrounded;
        [SerializeField] bool isOnWall;
        bool isOnWallsRightSide;
        bool isOnWallsLeftSide;
        bool isWallSliding;
        Vector3 zeroVelocity = Vector3.zero;
        public int wallDirX;
        [SerializeField] float speed = 1f;
        [SerializeField] float wallJumpForce = 1f;
        [SerializeField] float jumpForce = 1f;
        [SerializeField] int extraJumpCount;
        [SerializeField] int extraJumpValue = 1;
        [SerializeField] float wallSlidingSpeedMax = 2f;
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    
      
        [SerializeField] float rightPositionExtra = .2f;
       
        [SerializeField] LayerMask groundLayer;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            extraJumpCount = extraJumpValue;
         
        }
       
        private void Update()
        {
       
            Debug.DrawRay(transform.position, transform.right, Color.blue);
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + rightPositionExtra, transform.position.y), Color.green);
            isWallSliding = false;
            //Checkig is jumping here prevent additional unnecessary jumps added
            if (isOnSurface && !isJumping)
            {
                
                if (extraJumpCount < extraJumpValue) {
                    extraJumpCount = extraJumpValue;
                }            
            }
            if (isOnWall && !isGrounded)
            {
                isWallSliding = true;
                if (rb.velocity.y < -wallSlidingSpeedMax)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeedMax);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
            CheckingRayCasts();
           
        }
        private void FixedUpdate()
        {
            //   isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);
            
            Movement();
            // Jump();
            Jump();

        }
      
        void Movement()
        {
            move.x = Input.GetAxis("Horizontal");
          
            rb.velocity= new Vector2(move.x*speed, rb.velocity.y);
            // Vector2 targetVel = new Vector2(move.x * speed, rb.velocity.y);
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref zeroVelocity, m_MovementSmoothing);
            if (extraJumpCount > 0)
            {
                canJump = true;
            }
            else { canJump = false; }
        }
        void Jump()
        {
            if (isJumping )
            {
              //  int jumpcountBeforeJump = extraJumpCount;
                if (extraJumpCount > 0)// && !isOnWall
                {

                    if (isOnWall)
                    {
                        extraJumpCount--;
                           rb.velocity = new Vector2(rb.velocity.x * wallJumpForce, jumpForce);
                      //  Vector2 targetVel = new Vector2(-rb.velocity.x * wallJumpForce * speed, jumpForce);
                     //   rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref zeroVelocity, m_MovementSmoothing);      
                    }
                    else
                    {
                        extraJumpCount--;
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    }

                    //rb.AddForce(jumpHeight, ForceMode2D.Impulse);
                    // rb.AddForce(new Vector2(0f, jumpForce));
                }

               // extraJumpCount--;
              //  isJumping = false;
            }

        }

 
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                transform.parent = collision.transform;
            }

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //instead of Raycasts

    /*            Vector2 normal = collision.GetContact(0).normal;
            if (normal == (Vector2.up))
            {
                //Consider adding exception on sides of platform -solved with friction material
             //   Debug.Log("normal is vector2.up");
                isGrounded = true;
                isOnSurface = true;
            }
            if (normal == (Vector2.right) || normal == (Vector2.left))
            {
                //Consider adding exception on sides of platform -solved with friction material
           //     Debug.Log("is on Wall");
                isOnWall = true;
                isOnSurface = true;
                if (normal == (Vector2.right))
                {
                    isOnWallsRightSide = true;
                    wallDirX = -1;
                }
                if (normal == (Vector2.left))
                {
                      isOnWallsLeftSide = true;
                    wallDirX = 1;
                }
            }*/


        }
        private void OnCollisionExit2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Platform"))
            {
                transform.parent = null;
            }
        /*    isGrounded = false;
            isOnWall = false;
            isOnSurface = false;
            isOnWallsLeftSide = false;
            isOnWallsRightSide = false;*/
         
        }
        void CheckingRayCasts()
        {
            // bool hasRightHit = // if (hasRightHit)
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position,
                transform.right, rightPositionExtra, groundLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position,
                -transform.right, rightPositionExtra, groundLayer);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position,
                -transform.up, rightPositionExtra, groundLayer);

            if (hitRight)
            {
                isOnWall = true;
                isOnSurface = true;
                Debug.Log("Has hit Right");
                isOnWallsRightSide = true;
            }
            if (hitLeft)
            {
                isOnWall = true;
                isOnSurface = true;
                Debug.Log("Has hit left");
                isOnWallsLeftSide = true;
            }
            if (hitDown)
            {
                isGrounded = true;
                isOnSurface = true;
                Debug.Log("Has hit Down");
            }
            if (!hitDown && !hitRight && !hitLeft) { isOnSurface = false; }
            if(!hitRight && !hitLeft) { isOnWall = false; }
            if (!hitRight)
            {
                isOnWallsRightSide = false;
            }
            if (!hitLeft)
            {
               isOnWallsLeftSide = false;
            }
            if (!hitDown)
            {
                isGrounded = false;
            }
            /*    isGrounded = false;
                isOnWall = false;
                isOnSurface = false;
                isOnWallsLeftSide = false;
                isOnWallsRightSide = false;*/
        }

    }
}

/*
 *   [SerializeField] float rightPositionX;
        [SerializeField] float rightPositionXmin;
 *   [SerializeField] Vector2 jumpHeight;
[SerializeField] LayerMask groundMask;
[SerializeField] float checkRadius;
[SerializeField] Transform groundCheck;
 bool isHittingRight;
        BoxCollider2D col;
        RaycastHit2D rightHit;


     public enum JumpState
        {
            Grounded,
            PrepToJump,
            Jumping,
            InTheAir,
            Landed
        }
        JumpState jumpState = JumpState.Grounded;
*/
/*     void ChangeJumpState()
      {
          allowJump = false;
          switch (jumpState)
          {
              case JumpState.PrepToJump:
                  jumpState = JumpState.Jumping;
                  allowJump = true;
                  stopJump = false;
                  break;
              case JumpState.Jumping:
                  if (!isGrounded)
                  {
                      jumpState = JumpState.InTheAir;

                  }
                  break;
              case JumpState.InTheAir:
                  if (isGrounded)
                  {
                      jumpState = JumpState.Landed;
                  }
                  break;
              case JumpState.Landed:
                  jumpState = JumpState.Grounded;
                  break;
          }
      }*/
/*     [SerializeField] bool allowJump;
      [SerializeField] bool stopJump;*/

/*
     isAllowedToJump = true;

            playerBody = GetComponent<Rigidbody2D>();
            playerColider = GetComponent<BoxCollider2D>();
            rightPositionX = playerColider.bounds.max.x + .1f;
            topPositionY = playerColider.bounds.max.y + .1f;
            bottomPositionY = playerColider.bounds.min.y - .1f;
            leftPositionX = playerColider.bounds.min.x - .1f;


 */

/*    landingHit = 
        Physics2D.Raycast(new Vector2(this.transform.position.x, bottomPositionY + transform.position.y), 
        new Vector2(transform.position.x, 0.2f));
    leftHit = 
        Physics2D.Raycast(new Vector2(leftPositionX + transform.position.x, this.transform.position.y), 
        new Vector2(leftPositionX - 0.2f, 0.0f), 0.2f);
    rightHit = 
        Physics2D.Raycast(new Vector2(rightPositionX + transform.position.x, this.transform.position.y), 
        new Vector2(rightPositionX + 0.2f, 0.0f), 0.2f);
    topHit = 
        Physics2D.Raycast(new Vector2(this.transform.position.x, topPositionY + transform.position.y), 
        new Vector2(transform.position.x, 0.2f), 0.2f);

    Debug.DrawRay(new Vector2(rightPositionX + transform.position.x, this.transform.position.y), 
        new Vector2(rightPositionX + 0.2f, 0.0f), Color.black);

    if (landingHit.collider.tag == "Ground")
    {
        isAllowedToJump = true;
        Debug.Log("Hit the floor");
    }
    if (topHit.collider != null)
    {
        if (topHit.collider.tag == "Ground")
        {
            isAllowedToJump = false;
            Debug.Log("Hit the top");
        }
    }
    if (leftHit.collider != null)
    {
        if (leftHit.collider.tag == "Ground")
        {

            isOnWall = true;
        }

    }
    if (rightHit.collider != null)
    {
        if (rightHit.collider.tag == "Ground")
        {
            isOnWall = true;
        }
    }
    if (rightHit.collider == null && leftHit.collider == null)
    {
        isOnWall = false;
    }
    if (isOnWall)
    {
        isAllowedToJump = true;
    }*/
/*     else if (extraJumpCount > 0 && isOnWall )
           {
               Debug.Log("first teset");
               if ((move.x > 0 && isOnWallsLeftSide)|| (move.x < 0 && isOnWallsRightSide))
               {
                   Debug.Log("move.x is greater than zero and on left side");
                   rb.velocity = new Vector2(rb.velocity.x * -wallJumpForce, jumpForce);
                   extraJumpCount--;
               }
               if ((move.x < 0 && isOnWallsLeftSide )|| (move.x > 0 && isOnWallsRightSide))
               {
                   rb.velocity = new Vector2(rb.velocity.x * wallJumpForce, jumpForce);
                   extraJumpCount--;
               }
           }*/
/*                else if (extraJumpCount > 0)
                {

                    rb.velocity = new Vector2(rb.velocity.x * wallJumpForce, jumpForce);
                    extraJumpCount--;

                }*/
/*     private RaycastHit2D landingHit;
        private RaycastHit2D leftHit;
        private RaycastHit2D rightHit;
        private RaycastHit2D topHit;
        private BoxCollider2D playerColider;

        float rightPositionX;
        float topPositionY;
        float bottomPositionY;
        float leftPositionX;
        Rigidbody2D playerBody;*/
/*  if (isWallSliding)
                  {
                      if (wallDirX == move.x)
                      {
                          rb.velocity = new Vector2(-wallDirX * jumpClimb.x, jumpClimb.y);
                      }
                      else if (move.x == 0)
                      {
                          rb.velocity = new Vector2(-wallDirX * jumpOff.x, jumpOff.y);
                      }
                      else
                      {
                          rb.velocity = new Vector2(-wallDirX * wallLeap.x, wallLeap.y);
                      }

                  }
                  if (isGrounded)
                  {
                      rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                  }
                  extraJumpCount--;*/
/*   if (isOnSurface)
            {
                extraJumpCount = extraJumpValue;
            }
            if(isOnWall && !isGrounded)
            {
                isWallSliding = true;
                if(rb.velocity.y < -wallSlidingSpeedMax)
                {
                    rb.velocity= new Vector2(rb.velocity.x, -wallSlidingSpeedMax);
                }
            }*/
/*            if ((isOnWallsLeftSide||isOnWallsRightSide) && !isGrounded && rb.velocity.y < 0)
            {
                isWallSliding = true;
                if (rb.velocity.y < -wallSlidingSpeedMax)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeedMax);
                }
                if(timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    if(move.x !=wallDirX && move.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }

                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }}
*/
/*     rightPositionX = col.bounds.max.x + .1f;
            rightPositionXmin = col.bounds.min.x ;
            Debug.DrawRay(new Vector2(rightPositionX + transform.position.x, transform.position.y),
       new Vector2(rightPositionX +rightPositionExtra, 0.0f), Color.black);
 */
/*[SerializeField] Vector2 jumpClimb;
[SerializeField] Vector2 jumpOff;
[SerializeField] Vector2 wallLeap;
    float velocityXSmoothing;
        Vector2 velocity;

      public float timeToWallUnstick;
        [SerializeField] float wallStickTime = 0.3f;
*/