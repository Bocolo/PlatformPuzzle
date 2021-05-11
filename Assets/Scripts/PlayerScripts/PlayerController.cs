using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController : MonoBehaviour
    {

        public enum JumpState
        {
            Grounded,
            PrepToJump,
            Jumping,
            InTheAir,
            Landed
        }
        JumpState jumpState = JumpState.Grounded;
        Vector2 move;
       
        Rigidbody2D rb;
        [SerializeField] bool isGrounded;
        [SerializeField] bool allowJump;
        [SerializeField] bool stopJump;
        [SerializeField] Vector2 jumpHeight;
        [SerializeField] float speed = 1f;
        [SerializeField] float jumpForce = 1f;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
           
        }
        private void FixedUpdate()
        {
            Movement();
            Jump();
        }
        void Movement()
        {
            move.x = Input.GetAxis("Horizontal");
          
            rb.velocity= new Vector2(move.x*speed, rb.velocity.y);
          
        }
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                  rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //rb.AddForce(jumpHeight, ForceMode2D.Impulse);
               // rb.AddForce(new Vector2(0f, jumpForce));
            }
            
        }

        void ChangeJumpState()
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
