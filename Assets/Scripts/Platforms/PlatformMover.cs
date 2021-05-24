using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Player;

namespace Platformer.Platforms
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] float positionToMoveToX;
        [SerializeField] Vector2 startPosition;
        [SerializeField] Transform[] wayPoints;
        public float speed = 2f;
        [SerializeField] int currentWayPoint = 0;
        public bool isFacingRight = true;
        Rigidbody2D rb;
        [SerializeField] bool isMovedByRigidbody = false;

        bool playerOnTop;
        private void Start()
        {
         //   rb = GetComponent<Rigidbody2D>();
        
        }


        private void FixedUpdate()
        {


            if (transform.position != wayPoints[currentWayPoint].transform.position)
            {
                if (isMovedByRigidbody) {
                   /* Vector2 v = Vector2.MoveTowards(transform.position,
                         wayPoints[currentWayPoint].transform.position,
                         speed * Time.deltaTime);
                    rb.MovePosition(v);
                    Debug.Log("moved byrb");*/
          
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        wayPoints[currentWayPoint].transform.position,
                        speed * Time.deltaTime);
                }
            }

                SetCurrentPoint();

            if (transform.position.x < wayPoints[currentWayPoint].transform.position.x)
            {
                isFacingRight = true;
            }
            else { 
                isFacingRight = false; 
            }
     
        }
            
        void SetCurrentPoint()
        {
            if(transform.position == wayPoints[currentWayPoint].transform.position)
            {
                currentWayPoint += 1;
            }
            if(currentWayPoint >= wayPoints.Length)
            {
                currentWayPoint = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
          
        }
    }
}
/* 
 *   if(collision.gameObject.CompareTag("Player") && playerController == null)
            {
                playerController = collision.gameObject.GetComponent<PlayerController>();
                playerOnTop = true;

            }
 * if (isFacingRight)
            {
                Debug.Log("mov right");*//*
                rb.velocity = new Vector2(1 * speed, rb.velocity.y);*//*
                //    rb.MovePosition(transform.position = new Vector3(1, 0, 0) * Time.deltaTime * speed);
                Vector2 v = Vector2.MoveTowards(transform.position,
                   wayPoints[currentWayPoint].transform.position,
                   speed * Time.deltaTime);
                rb.MovePosition(v);
            }
            else
            {
                Debug.Log("mov left");
                Vector2 v = Vector2.MoveTowards(transform.position,
              wayPoints[currentWayPoint].transform.position,
              speed * Time.deltaTime);
                rb.MovePosition(v);
                *//*   rb.velocity = new Vector2(-1 * speed, rb.velocity.y);*//*
                //  rb.MovePosition(transform.position = new Vector3(-1, 0, 0) * Time.deltaTime * speed);
            }

                                                                                      Vector2 newPos = transform.position;
            Vector2 ded = newPos - oldPos;
            Vector2 vel = ded / Time.deltaTime;
            if(playerOnTop && playerController != null)
            {
                playerController.AddVelocity(vel);
            }
                                                                               
                                                                            oldPos = transform.position;
            Vector2 oldPos;
            PlayerController playerController;       */