﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController : MonoBehaviour
    {    
        [SerializeField] float speed = 4f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float wallSlidingSpeedMax = 2f;        
        [SerializeField] int extraJumpValue = 1;  
        [SerializeField] LayerMask groundLayer;
        [SerializeField] Transform groundCheck;
        [SerializeField] Transform wallCheckLeft;
        [SerializeField] Transform wallCheckRight;    
        [SerializeField] Vector2 wallCubeSize;
        [SerializeField] Vector2 groundCubeSize;
        [SerializeField] Vector2 wallForce;
        [SerializeField] float wallJumpTime= .05f;
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
        Vector3 zeroVelocity = Vector3.zero;
        Vector2 move;
        Rigidbody2D rb;
        int extraJumpCount;
        bool isOnGround = false;
        bool isOnRightWall = false;
        bool isOnLeftWall = false;
        bool isOnASurface = false;
        bool isAllowedToWallJump = false;
        bool isJumping;
        bool isWallSliding;
        int wallDirX;
        public bool canPlayerMove = true;
        public bool isActivePlayer = true;
        PlayerDie player;
         public Vector2 lastPosition;
        RigidbodyConstraints2D originalRbConstraints;
        RigidbodyConstraints2D inActiveConstraints;
   
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            extraJumpCount = extraJumpValue;
            player = GetComponent<PlayerDie>();
            originalRbConstraints = rb.constraints;
            inActiveConstraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }
       
        private void Update()
        {
            move.x = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!player.isDead && isActivePlayer )//&& canPlayerMove)
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
  
            if (isOnASurface && !isJumping)
            {
                SetJumpValue();         
            }          
        
            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeedMax, float.MaxValue));
            }

       
            if (!player.isDead && isActivePlayer)
                {
                    Movement(move);
                    WallJump();
                 }
            SetRbConstraints();
         
            /*TESTCODE*/
            SetWallSliding();
            SetSurfaceBools();  
            CheckingWallDirection();
        }
      
        void SetRbConstraints()
        {
            if (!isActivePlayer && rb.constraints != inActiveConstraints)
            {
                rb.constraints = inActiveConstraints;
                Debug.Log("setting inactive");

            }
            else if (isActivePlayer && rb.constraints != originalRbConstraints)
            {
                rb.constraints = originalRbConstraints;
                Debug.Log("setting active");
            }
        }
        void Movement(Vector2 move)
        {
         // rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
           Vector2 targetVel = new Vector2(move.x * speed, rb.velocity.y);
           rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref zeroVelocity, m_MovementSmoothing);
        }

        void Jump()
        {
            if((isOnGround || extraJumpCount > 0) && !isWallSliding)
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
        /*    Gizmos.color = Color.white;
        
            Gizmos.DrawWireCube(wallCheckLeft.position, wallCubeSize);
            Gizmos.DrawWireCube(wallCheckRight.position, wallCubeSize);
            Gizmos.DrawWireCube(groundCheck.position, groundCubeSize);*/
        }
         
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                transform.parent = collision.transform;
            }

        }

      
        private void OnCollisionExit2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Platform"))
            {
                transform.parent = null;
            }


        }
      

    }
}

/*        isGrounded = false;
        isOnWall = false;
        isOnSurface = false;
        isTouchingLeftWall = false;
        isTouchingRightWall = false;*/
/*   if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || extraJumpCount>0) && !isWallSliding)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumpCount--;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isWallSliding)
            {
                wallJumping = true;
                Invoke("SetWallJumpingToFalse", wallJumpTime);
            }
            if (wallJumping)
            {
                rb.velocity = new Vector2(xWallForce * -move.x, yWallForce);
            }

 
 
 
*/

/* ORIGINAL CODE             */
/*       if (!player.isDead)// && canPlayerMove)
                   {
                      Movement(move);
                       WallJump();
                   }*/
/* ORIGINAL CODE             */
/*    if (!canPlayerMove)
      {
          transform.position = lastPosition;
      }*/
//old variables
/*
 * 
 *  [SerializeField] float wallJumpAcrossForce;

 
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

       
      bool canJump;

        [SerializeField] bool isOnSurface;
        [SerializeField] bool isGrounded;
        [SerializeField] bool isOnWall;
        bool isTouchingRightWall;
        bool isTouchingLeftWall;
        [SerializeField] float rightPositionExtra = .2f;
        Vector3 zeroVelocity = Vector3.zero;
        [SerializeField] float groundRadius = .2f;
        [SerializeField] float wallRadius = .2f;

        [SerializeField] float wallJumpForce = 1f;
*/
//old contact point check

/*
   void CheckingContactPoints(Collision2D collision)
        {
            Vector2 normal = collision.GetContact(0).normal;
            if (normal == (Vector2.up))
            {
    
                isGrounded = true;
                isOnSurface = true;
            }
            if (normal == (Vector2.right) || normal == (Vector2.left))
            {
                isOnWall = true;
                isOnSurface = true;
                if (normal == (Vector2.right))
                {
                    isTouchingRightWall = true;
                    wallDirX = -1;
                }
                if (normal == (Vector2.left))
                {
                    isTouchingLeftWall = true;
                    wallDirX = 1;
                }
            }
        }
       
 */
//Old rayCast check
/*
 
 
 
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
                isTouchingRightWall = true;
                wallDirX = -1;
            }
            if (hitLeft)
            {
                isOnWall = true;
                isOnSurface = true;
                Debug.Log("Has hit left");
                isTouchingLeftWall = true;
                wallDirX = 1;
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
                isTouchingRightWall = false;
            }
            if (!hitLeft)
            {
               isTouchingLeftWall = false;
            }
            if (!hitDown)
            {
                isGrounded = false;
            }
               isGrounded = false;
                isOnWall = false;
                isOnSurface = false;
                isTouchingLeftWall = false;
                isTouchingRightWall = false;
        }
 
 */