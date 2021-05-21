using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool hasJoint = false;
        FixedJoint2D fj;
        [SerializeField] int breakForce;
        [SerializeField] float speed = 4f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float wallSlidingSpeedMax = 2f;
        [SerializeField] float wallJumpTime = .05f;
        [SerializeField] int extraJumpValue = 1;  
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask otherPlayerLayer;
        [SerializeField] LayerMask playerLayer;     
        [SerializeField] Transform groundCheck;
        [SerializeField] Transform topCheck;
        [SerializeField] Transform wallCheckLeft;
        [SerializeField] Transform wallCheckRight;    
        [SerializeField] Vector2 wallCubeSize;
        [SerializeField] Vector2 groundCubeSize;
        [SerializeField] Vector2 wallForce;
     
        [SerializeField] SpriteRenderer activeSprite ;
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
        Vector3 zeroVelocity = Vector3.zero;
        Vector2 move;
        Rigidbody2D rb;
        int extraJumpCount;
        bool isOnGround = false;
        bool isOnRightWall = false;
        bool isOnLeftWall = false;
        bool isTouchingPlayerLeft = false;
        bool isTouchingPlayerRight = false;
        bool isOnASurface = false;
        bool isAllowedToWallJump = false;
        bool isJumping;
        bool isWallSliding;
        int wallDirX;       
        public bool isActivePlayer = false;
        [SerializeField] bool isAnotherPlayerOnTop;
        [SerializeField] bool isOnTopOfOtherPlayer;
        PlayerController playerOnTop;
        PlayerDie player;        
        RigidbodyConstraints2D originalRbConstraints;
        RigidbodyConstraints2D inActiveConstraints;
        RigidbodyConstraints2D onTopOfPlayerConstraints;
        RigidbodyConstraints2D inActiveConstraintsAndGrounded;
        public bool isOnTopAndShouldMove = false;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            extraJumpCount = extraJumpValue;
            player = GetComponent<PlayerDie>();
            originalRbConstraints = rb.constraints;
            inActiveConstraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            onTopOfPlayerConstraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            inActiveConstraintsAndGrounded = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
           

        }
       
        private void Update()
        { 
            move.x = Input.GetAxis("Horizontal");
       
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!player.isDead && isActivePlayer && !isAnotherPlayerOnTop)
                {
                    isJumping = true;
                    Jump();
                    SetWallJumping();
                }
            }
            else
            {
                isJumping = false;
            }
            if ((isOnASurface && !isJumping)|| isOnTopOfOtherPlayer) 
            {
                SetJumpValue();         
            }                 
            if (isWallSliding && rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeedMax, float.MaxValue));
            }       
            if (!player.isDead && isActivePlayer && rb!=null)
                {
                    Movement(move);
                    WallJump();
                 }
            if (isActivePlayer)
            {
                isOnTopAndShouldMove = false;
            }
       
            SetActiveSprite();
            SetWallSliding();
            SetSurfaceBools();  
            CheckingWallDirection();
       
            SetRbConstraintsForJoints();
            if (isActivePlayer)
            {
                if (fj != null)
                {
                    // fj = null;
                    Destroy(GetComponent<FixedJoint2D>());
                    hasJoint = false;
                    Debug.Log("REMOVING FJ0");
                }
            }
            if (fj == null && hasJoint)
            {
                hasJoint = false;
                Debug.Log("change joint bool to false");
            }
        }
       
        private void FixedUpdate()
        {
         /*       if (((Mathf.Round(oldtransform.x * 100)) / 100 == (Mathf.Round(transform.position.x * 100)) / 100) || move.x==0)
                {
                
                    ismoving = false;
                }
                else
                {
                    oldtransform = transform.position;
       
                    ismoving = true;
                }*/
        }

        void SetRbConstraintsForJoints()
        {
            if (!isActivePlayer)
            {
                if (isOnGround && rb.constraints != inActiveConstraintsAndGrounded && !isOnTopOfOtherPlayer)
                {
                    rb.constraints = inActiveConstraintsAndGrounded;
                }
                else if (isOnTopOfOtherPlayer && rb.constraints != onTopOfPlayerConstraints &&!hasJoint)
                {
                    rb.constraints = onTopOfPlayerConstraints;
                }
                else if(isOnTopOfOtherPlayer && rb.constraints != originalRbConstraints && hasJoint)
                {
                    rb.constraints = originalRbConstraints;
                }
                else if (!isOnGround && rb.constraints != inActiveConstraints && !isOnTopOfOtherPlayer)
                {
                    rb.constraints = inActiveConstraints;
                }

            }
            else if (isActivePlayer && (rb.constraints == inActiveConstraints || rb.constraints == inActiveConstraintsAndGrounded || rb.constraints == onTopOfPlayerConstraints))
            {
                rb.constraints = originalRbConstraints;
            }
        }
        void SetRbConstraints()
        {        
            if (!isActivePlayer)// && (rb.constraints != inActiveConstraints || rb.constraints != inActiveConstraintsAndGrounded)
            {             
                if (isOnGround && rb.constraints != inActiveConstraintsAndGrounded && !isOnTopOfOtherPlayer)// && !isOnTopAndShouldMove)
                {
                    rb.constraints = inActiveConstraintsAndGrounded;
                }
                else if (isOnTopAndShouldMove && rb.constraints != onTopOfPlayerConstraints) // && rb.constraints != originalRbConstraints)// && isOnTopAndShouldMove
                {
                    rb.constraints = onTopOfPlayerConstraints;
                }
                else if(!isOnGround && rb.constraints != inActiveConstraints &&!isOnTopAndShouldMove)// && !isOnTopOfOtherPlayer)
                {
                    rb.constraints = inActiveConstraints;
                }

            }
            else if (isActivePlayer && (rb.constraints == inActiveConstraints || rb.constraints == inActiveConstraintsAndGrounded ||rb.constraints == onTopOfPlayerConstraints))
            {
                rb.constraints = originalRbConstraints;
            }
        }
        public void Movement(Vector2 move)
        {
          rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
      //     Vector2 targetVel = new Vector2(move.x * speed, rb.velocity.y);
        //   rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref zeroVelocity, m_MovementSmoothing);

       /*     if (playerOnTop != null && ismoving)
            {
                //   playerOnTop.transform.position = new Vector3(gameObject.transform.position.x - distanceXY, playerOnTop.transform.position.y, 0);
                playerOnTop.isOnTopAndShouldMove = true;
                Debug.Log("SHOULD M");
            }
            if (playerOnTop != null && !ismoving)
            {
                Debug.Log("SHOULD not move");
                playerOnTop.isOnTopAndShouldMove = false;
            }*/
      
        }
/*            public void MoveWhenOnTop(Vector2 move)
        {
            Debug.Log("SHOULD MOVE on top");
            Vector2 targetVel = new Vector2(move.x * speed, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref zeroVelocity, m_MovementSmoothing);

        }*/
        void Jump()
        {
            if(((isOnGround || extraJumpCount > 0) && !isWallSliding)|| isOnTopOfOtherPlayer)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumpCount--;
            }
        }
        void WallJump()
        {
            if (isAllowedToWallJump) //&&(move.x == wallDirX || move.x ==0)
            {
                rb.velocity = new Vector2(wallForce.x * -wallDirX, wallForce.y);//-move.x//wallDirX
            }

        }
        void SetWallSliding()
        {
            if ((isOnLeftWall || isOnRightWall) && !isOnGround)
            {
                isWallSliding = true;
            }
            else { isWallSliding = false; }
        }
        void SetWallJumping()
        {
            if (isWallSliding )
            {
                isAllowedToWallJump = true;
                Invoke("SetWallJumpingToFalse", wallJumpTime);
            }
        }
        void SetActiveSprite()
        {
            if (isActivePlayer)
            {
                activeSprite.enabled = true;
            }
            else
            { activeSprite.enabled = false; }
        }
        void SetWallJumpingToFalse()
        {
            isAllowedToWallJump = false;
        }
        void SetJumpValue()
        {
            if (extraJumpCount < extraJumpValue)
            {
                extraJumpCount = extraJumpValue;
            }
        }
                        
        void SetSurfaceBools()
        {
            if (isOnLeftWall || isOnRightWall || isOnGround)
            {
                isOnASurface = true;
            }
            else
            {
                isOnASurface = false;
            }
            isOnGround = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, groundLayer);
            isOnRightWall = Physics2D.OverlapBox(wallCheckRight.position, wallCubeSize, 0, groundLayer);
            isOnLeftWall = Physics2D.OverlapBox(wallCheckLeft.position, wallCubeSize, 0, groundLayer);

            if (isActivePlayer)
            {
                isOnTopOfOtherPlayer = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, otherPlayerLayer);
                isAnotherPlayerOnTop = Physics2D.OverlapBox(topCheck.position, groundCubeSize, 0, otherPlayerLayer);

                isTouchingPlayerLeft = Physics2D.OverlapBox(wallCheckLeft.position, wallCubeSize, 0, otherPlayerLayer);
                isTouchingPlayerRight = Physics2D.OverlapBox(wallCheckRight.position, wallCubeSize, 0, otherPlayerLayer);

            }
            else
            {
                
                    if (Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, playerLayer) || Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, otherPlayerLayer))
                    {
                        isOnTopOfOtherPlayer = true;
                    }
                    else
                    {
                        isOnTopOfOtherPlayer = false;
                    }
               // isOnTopOfOtherPlayer = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0,playerLayer);
                isAnotherPlayerOnTop = Physics2D.OverlapBox(topCheck.position, groundCubeSize, 0, playerLayer);

                isTouchingPlayerLeft = Physics2D.OverlapBox(wallCheckLeft.position, wallCubeSize, 0, playerLayer);
                isTouchingPlayerRight = Physics2D.OverlapBox(wallCheckRight.position, wallCubeSize, 0, playerLayer);
            }


        }
        void CheckingWallDirection()
        {
            if (isOnLeftWall)
            {
                wallDirX = -1;

            }
            else if (isOnRightWall)
            {
                wallDirX = 1;

            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            Gizmos.DrawWireCube(wallCheckLeft.position, wallCubeSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(wallCheckRight.position, wallCubeSize);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCubeSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(topCheck.position, groundCubeSize);

        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                transform.parent = collision.transform;
            }
            if (collision.gameObject.CompareTag("Player") && isOnTopOfOtherPlayer && !isActivePlayer && !hasJoint)
            {
              
                hasJoint = true;
                /*   fj = collision.gameObject.AddComponent<FixedJoint2D>();
                   fj.connectedBody = rb;
   */
                fj = gameObject.AddComponent<FixedJoint2D>();
                fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                fj.enableCollision = true;
                fj.breakForce = breakForce;

            }
      
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
 
            if (collision.gameObject.CompareTag("Platform"))
            {
                transform.parent = null;
            }
      /*      if (collision.gameObject.CompareTag("Player") && !isOnTopOfOtherPlayer && hasJoint)
            {
           
                if (fj != null)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    hasJoint = false;
                    Debug.Log("Working on exit");
                }
         

            }*/


        }
  

    }
}



