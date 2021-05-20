using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool ismoving;
        [SerializeField] float speed = 4f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float wallSlidingSpeedMax = 2f;        
        [SerializeField] int extraJumpValue = 1;  
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask otherPlayerLayer;
        [SerializeField] LayerMask playerLayer;
        bool isDifSelected = false;
        [SerializeField] Transform groundCheck;
        [SerializeField] Transform topCheck;
        [SerializeField] Transform wallCheckLeft;
        [SerializeField] Transform wallCheckRight;    
        [SerializeField] Vector2 wallCubeSize;
        [SerializeField] Vector2 groundCubeSize;
        [SerializeField] Vector2 wallForce;
        [SerializeField] float wallJumpTime= .05f;
        [SerializeField] SpriteRenderer activeSprite ;
        [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
        Vector3 zeroVelocity = Vector3.zero;
        Vector2 move;
        Rigidbody2D rb;
        int extraJumpCount;
        bool isOnGround = false;
        bool isOnRightWall = false;
        bool isOnLeftWall = false;
       public bool isTouchingPlayerLeft = false;
      public  bool isTouchingPlayerRight = false;
        bool isOnASurface = false;
        bool isAllowedToWallJump = false;
        bool isJumping;
        bool isWallSliding;
        int wallDirX;
        public bool canPlayerMove = true;
        public bool isActivePlayer = false;
        [SerializeField] bool isAnotherPlayerOnTop;
        [SerializeField] bool isOnTopOfOtherPlayer;
        PlayerController playerOnTop;
        PlayerDie player;
         public Vector3 lastPosition;
        RigidbodyConstraints2D originalRbConstraints;
        RigidbodyConstraints2D inActiveConstraints;
        RigidbodyConstraints2D inActiveConstraintsAndGrounded;
        public bool lastPositionIstransform =false;
        public Vector2 playerUnderneatLastPosition;
        Vector3 oldtransform;
        public bool isOnTopAndShouldMove = false;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            extraJumpCount = extraJumpValue;
            player = GetComponent<PlayerDie>();
            originalRbConstraints = rb.constraints;
            inActiveConstraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            inActiveConstraintsAndGrounded = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            oldtransform = transform.position;

        }
       
        private void Update()
        { 
            move.x = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!player.isDead && isActivePlayer && !isAnotherPlayerOnTop)//&& canPlayerMove) //&& !isAnotherPlayerOnTop is part of temporary solution with movement
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
            if ((isOnASurface && !isJumping)|| isOnTopOfOtherPlayer) /// (isOnASurface && !isJumping) original . temp changed for moving with play on tope
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
            if (isActivePlayer)
            {
                isOnTopAndShouldMove = false;
            }
            SetRbConstraints();
            SetActiveSprite();
            SetWallSliding();
            SetSurfaceBools();  
            CheckingWallDirection();
            SetArrayOfPlayers();
    
       
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
        void SetRbConstraints()
        {        
            if (!isActivePlayer)// && (rb.constraints != inActiveConstraints || rb.constraints != inActiveConstraintsAndGrounded)
            {             
                if (isOnGround && rb.constraints != inActiveConstraintsAndGrounded)// && !isOnTopAndShouldMove)
                {
                    rb.constraints = inActiveConstraintsAndGrounded;
                }
                else if (isOnTopAndShouldMove && rb.constraints != originalRbConstraints)// && isOnTopAndShouldMove
                {
                    rb.constraints = originalRbConstraints;
                }
                else if(!isOnGround && rb.constraints != inActiveConstraints &&!isOnTopAndShouldMove)// && !isOnTopOfOtherPlayer)
                {
                    rb.constraints = inActiveConstraints;
                }

            }
            else if (isActivePlayer && (rb.constraints == inActiveConstraints || rb.constraints == inActiveConstraintsAndGrounded))
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
        void SetArrayOfPlayers()
        {
            Collider2D[] listOfPlayersOnTop;
            if (isActivePlayer)
            {
                listOfPlayersOnTop = Physics2D.OverlapBoxAll(topCheck.position, groundCubeSize, 0, otherPlayerLayer);
                Debug.Log(listOfPlayersOnTop.Length);
                for (int i = 0; i < listOfPlayersOnTop.Length; i++)
                {
                    PlayerController pc = listOfPlayersOnTop[i].GetComponent<PlayerController>();
                    pc.isOnTopAndShouldMove = true;
                  
                    //HAVE TO DO A MOVEMENT CHECK -- IS CURRENT PLAYER STILL MOVING, STALL MOVEMENT

                    if (((!isOnLeftWall || !isTouchingPlayerLeft) && move.x < 0) || ((!isOnRightWall || !isTouchingPlayerRight) && move.x > 0))
                    {
                        pc.Movement(move);
                      
                        pc.isOnTopAndShouldMove = true;
                    }
                    if (((isOnLeftWall || isTouchingPlayerLeft) && move.x < 0) || ((isOnRightWall || isTouchingPlayerRight) && move.x > 0))
                    {
                        pc.isOnTopAndShouldMove = false;
                    }
                }
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
                isOnTopOfOtherPlayer = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0,playerLayer);
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


       /*     if (collision.gameObject.CompareTag("Player")
         && isActivePlayer
         && isAnotherPlayerOnTop
         && !collision.gameObject.GetComponent<PlayerController>().isActivePlayer
         )
            {
                playerOnTop = collision.gameObject.GetComponent<PlayerController>();
                Debug.Log("sasda");
            }*/

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
/*         /* 
 *         
 *            if (collision.gameObject.CompareTag("Player") && !isActivePlayer && !isOnTopOfOtherPlayer)
            {
                distanceXY = 0;
            }
if(isActivePlayer && rb == null)
            {*//*
                gameObject.AddComponent<Rigidbody2D>();
                rb = GetComponent<Rigidbody2D>();*//*

                rb = gameObject.AddComponent<Rigidbody2D>();
               //GetComponent<Rigidbody2D>();
                Debug.Log("Active player asdding rb");
            }*/
/* if (collision.gameObject.CompareTag("Player")
         && isActivePlayer
         && isAnotherPlayerOnTop
         && !collision.gameObject.GetComponent<PlayerController>().isActivePlayer
         )
        {
            //   Vector3 difference = 
            playerOnTop = collision.gameObject.GetComponent<PlayerController>();
            if (distanceXY == 0)
            {
                distanceXY = Mathf.Abs(collision.transform.position.x) - Mathf.Abs(gameObject.transform.position.x);
                minX = Mathf.Abs(collision.transform.position.x) - Mathf.Abs(wallCheckLeft.transform.position.x);
                maxX = Mathf.Abs(collision.transform.position.x) - Mathf.Abs(wallCheckRight.transform.position.x);
             *//*   Debug.Log(minX + "  : minx");
                Debug.Log(maxX + "  : max");*/
/*    Debug.Log(gameObject.transform.position.x - minX);
    Debug.Log(gameObject.transform.position.x - maxX);
    Debug.Log(distanceXY + "  : dist");

    Debug.Log(gameObject.transform.position.x - distanceXY);
*/
/*   float leftMax = playerOnTop.wallCheckLeft.transform.position.x;
   float rightMax = playerOnTop.wallCheckRight.transform.position.x;
   Debug.Log(leftMax + "left");
   Debug.Log(leftMax + collision.transform.position.x + "  left max");
   float dif = collision.transform.position.x - leftMax;
   float halfdif = dif / 2;
   Debug.Log(rightMax + "  right");

   Debug.Log(wallCheckRight.transform.position.x + ".  Right X on bottom");
   Debug.Log(wallCheckLeft.transform.position.x + ".  Left X on bottom");*//*
}

//Set the diffence into 
}*/
/*gameObject.transform.parent = null;
            if (rb == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
                rb = GetComponent<Rigidbody2D>();
                Debug.Log("not on top of player adding Rigidbody");
            }
        }*/
/*      if (collision.gameObject.CompareTag("Player") && !isActivePlayer && isOnTopOfOtherPlayer)// &&!isOnASurface)
          {
              gameObject.transform.parent = collision.gameObject.transform;
              Destroy(GetComponent<Rigidbody2D>());
              Debug.Log("onPlayer removing rigidbody");
              rb = null;
          }*/
/*

  if (!isDifSelected)
  {
 //     List<float> diffArray = new List<float>();
      //Set a value for active player
       posX = transform.position.x;
      Debug.Log("psx: " + posX);
       bottomX = collision.transform.position.x;
      Debug.Log("bptx: " + bottomX);
      xDif = posX - bottomX;
      if (xDif != 0)
      {
          isDifSelected = true;
          origTransform= transform.position;
      }
  }
  gameObject.transform.parent = collision.gameObject.transform;
  rb.bodyType = RigidbodyType2D.Kinematic;

  Debug.Log("difx: " + xDif);
       }
*/
/*            if (isActivePlayer && rb.bodyType != RigidbodyType2D.Dynamic)
             {
                 rb.bodyType = RigidbodyType2D.Dynamic;
             }
             if(!isActivePlayer && rb.bodyType != RigidbodyType2D.Dynamic && isOnASurface)
             {
                 rb.bodyType = RigidbodyType2D.Dynamic;
                 Debug.Log("surafce contact rigidbody reset");
             }*/
/*
  if(lastPosition != transform.position)
  {
      lastPosition = transform.position;
      Debug.Log("last position is set");
  }
  if(lastPosition == transform.position) && not moving
  {
      lastPositionIstransform = true;
  }
  else
  {
      lastPositionIstransform = false;
      //using last pos X as max X movement
  }*/
/*     if(!isActivePlayer && isOnTopOfOtherPlayer)
     {
         Movement(move);
     }*/
//Temporary solution -- allows player to move with active player 
//fault -- contact with other objects will push it
//is kinermatic on child rb works but will go through walls if overlapping

/* if (!player.isDead && !isActivePlayer && isOnTopOfOtherPlayer)
 {
     Movement(move);
 }*/
/*    // if (transpos ==transform.position) //.x == (Mathf.Round(transform.position.x *100))/100)
            if((Mathf.Round(transpos.x * 100)) / 100 == (Mathf.Round(transform.position.x * 100)) / 100)
            {

                Debug.Log("sdsdsd");
                ismoving = false;
            }
            else
            {

                transpos = transform.position;
             //     transpos.x = transform.position.x;
                //transpos.x = (Mathf.Round(transform.position.x * 100)) / 100;
                Debug.Log(transpos.x);
                ismoving = true;
            }*/