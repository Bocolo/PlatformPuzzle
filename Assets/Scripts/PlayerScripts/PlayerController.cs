using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController : MonoBehaviour
    {
       // [HideInInspector]
        public bool hasJoint = false;
        // [HideInInspector]
        public bool isOnTopOfOtherPlayer;
        //Choosing to go with Game option B-- multiplayer puzzle.
        //Massive Refactoring to do
        [SerializeField] float speed = 4f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float wallSlidingSpeedMax = 2f;
        [SerializeField] float wallJumpTime = .05f;
        [SerializeField] int extraJumpValue = 1;  
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask otherPlayerLayer;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] LayerMask platformLayer;
        [SerializeField] Transform groundCheck;
        [SerializeField] Transform topCheck;
        [SerializeField] Transform wallCheckLeft;
        [SerializeField] Transform wallCheckRight;    
        [SerializeField] Vector2 wallCubeSize;
        [SerializeField] Vector2 groundCubeSize;
        [SerializeField] Vector2 wallForce;     
        [SerializeField] SpriteRenderer activeSprite ;

        [HideInInspector]
        public bool isActivePlayer = false;

               [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
              Vector3 zeroVelocity = Vector3.zero;

        int extraJumpCount;     
         public bool isOnGround = false;
        bool isOnRightWall = false;
        bool isOnLeftWall = false;
        bool isTouchingPlayerLeft = false;
        bool isTouchingPlayerRight = false;
        bool isOnASurface = false;
        bool isAllowedToWallJump = false;
        bool isJumping;
        public bool isWallSliding;
        public bool isAnotherPlayerOnTop;
        int wallDirX;
        Vector2 move;
        [HideInInspector]
        public Rigidbody2D rb;
        PlayerDie player;        
        RigidbodyConstraints2D originalRbConstraints;
        RigidbodyConstraints2D inActiveConstraints;
        RigidbodyConstraints2D onTopOfPlayerConstraints;
        RigidbodyConstraints2D inActiveConstraintsAndGrounded;
        public bool playerIsOnPlatform = false;

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
            SetActiveSprite();
            SetWallSliding();
            SetSurfaceBools();  
            CheckingWallDirection();       
            SetRbConstraintsForJoints();        
        }
        private void FixedUpdate()
        {
            if (!player.isDead && isActivePlayer)
            {
                Movement(move);
                WallJump();
            }
            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeedMax, float.MaxValue));
            }
        }

        void SetRbConstraintsForJoints()
        {
            if (!isActivePlayer)
            {
                if (isOnGround && rb.constraints != inActiveConstraintsAndGrounded && !isOnTopOfOtherPlayer)
                {
                    rb.constraints = inActiveConstraintsAndGrounded;
                }
                else if (isOnTopOfOtherPlayer && rb.constraints != originalRbConstraints && hasJoint)
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
     
        public void Movement(Vector2 move)
        {
        //  rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
           Vector2 targetVel = new Vector2(move.x * speed, rb.velocity.y);
           rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref zeroVelocity, m_MovementSmoothing);
        }
        void Jump()
        {
            if(((isOnGround || playerIsOnPlatform || extraJumpCount > 0) && !isWallSliding)|| isOnTopOfOtherPlayer)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumpCount--;
            }
        }
        void WallJump()
        {
            if (isAllowedToWallJump) 
            {
                rb.velocity = new Vector2(wallForce.x * -wallDirX, wallForce.y);
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
          //  isOnGround = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, groundLayer);
            isOnRightWall = Physics2D.OverlapBox(wallCheckRight.position, wallCubeSize, 0, groundLayer);
            isOnLeftWall = Physics2D.OverlapBox(wallCheckLeft.position, wallCubeSize, 0, groundLayer);
            playerIsOnPlatform = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, platformLayer);
            if (isActivePlayer)
            {
              
                isAnotherPlayerOnTop = Physics2D.OverlapBox(topCheck.position, groundCubeSize, 0, otherPlayerLayer);

                isTouchingPlayerLeft = Physics2D.OverlapBox(wallCheckLeft.position, wallCubeSize, 0, otherPlayerLayer);
                isTouchingPlayerRight = Physics2D.OverlapBox(wallCheckRight.position, wallCubeSize, 0, otherPlayerLayer);

            }
            else
            {
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
            if (collision.contacts.Length > 0  && !isOnTopOfOtherPlayer && !isOnGround)
            {
               
                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    if (Vector3.Dot(collision.contacts[i].normal, Vector3.up) > 0.5)
                    {
                        Debug.Log("cont on bottom top");
              
                        if (collision.gameObject.CompareTag("Player") && !isOnTopOfOtherPlayer)
                        {
                            Debug.Log("is on top of another player");
                            isOnTopOfOtherPlayer = true;
                        }
                    }
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Length > 0 && !isOnGround)
            {
                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    if (Vector3.Dot(collision.contacts[i].normal, Vector3.up) > 0.5)
                    {
                        if (collision.gameObject.CompareTag("Ground") && !isOnGround)
                        {
                            isOnGround = true;
                        }
                    }
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = false;
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                isOnTopOfOtherPlayer = false;
            }
        }

    }
}



/*
         void SetRbConstraintsForJoints()
        {
            if (!isActivePlayer)
            {
                if (isOnGround && rb.constraints != inActiveConstraintsAndGrounded && !isOnTopOfOtherPlayer && !playerIsOnPlatform)
                {
                    rb.constraints = inActiveConstraintsAndGrounded;
                }
                else if ((isOnTopOfOtherPlayer|| playerIsOnPlatform) && rb.constraints != onTopOfPlayerConstraints &&!hasJoint )
                {
                    rb.constraints = onTopOfPlayerConstraints;
                }
                else if((isOnTopOfOtherPlayer|| playerIsOnPlatform) && rb.constraints != originalRbConstraints && hasJoint)
                {
                    rb.constraints = originalRbConstraints;
                }
                else if (!isOnGround && rb.constraints != inActiveConstraints && !isOnTopOfOtherPlayer && !playerIsOnPlatform)
                {
                    rb.constraints = inActiveConstraints;
                }

            }
            else if (isActivePlayer && (rb.constraints == inActiveConstraints || rb.constraints == inActiveConstraintsAndGrounded || rb.constraints == onTopOfPlayerConstraints))
            {
                rb.constraints = originalRbConstraints;
            }
        }    */