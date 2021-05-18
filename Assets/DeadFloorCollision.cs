using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.DeadPlayer
{
    public class DeadFloorCollision : MonoBehaviour
    {
        Rigidbody2D rb;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundLayer;
        public bool isGrounded = false;
        [SerializeField] Vector2 groundCubeSize;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, groundLayer);
            if (!isGrounded)
            {
                isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCubeSize, 0, groundLayer);
                rb.bodyType = RigidbodyType2D.Dynamic;
                Debug.Log("isgronding dead spritw");
            }
            if (isGrounded)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                Debug.Log("isgronding kimew");
            }
        }
    }
}
