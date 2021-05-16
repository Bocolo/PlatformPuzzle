using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Platforms;
using Platformer.Player;

namespace Platformer.Enemy
{
    public class EnemyRoamer : MonoBehaviour
    {
    
        [SerializeField] LayerMask playerLayer;
        [SerializeField] float speedUp;
        float originalSpeed;
        [SerializeField] bool isHittingPlayer;
        [SerializeField] bool canKillPlayer; 
        [SerializeField] float distanceToChase;
        [SerializeField] float distanceToKill;
        PlatformMover platform;
        PlayerDie player;
        [SerializeField] bool isBallEnemy =false;
        private void Start()
        {
            platform = GetComponent<PlatformMover>();
           // player = FindObjectOfType<PlayerDie>();
            originalSpeed = platform.speed;
        }
        private void Update()
        {
        //    CheckIsPlayerInViewLine();
            CheckWillPlayerBeKilled();
            ChasePlayer();
        }
        private void FixedUpdate()
        {
     
            Flip(platform.isFacingRight);
            CheckRayCasts();
        }
        void ChasePlayer()
        {
            if (isHittingPlayer)
            {
                platform.speed = speedUp;
            }
            else
            {
                platform.speed = originalSpeed;
            }
        }
     
        void CheckWillPlayerBeKilled()
        {
        //    canKillPlayer = Physics2D.OverlapBox(bottomCheck.position, bottomCubeSize, 0, playerLayer);
            if (canKillPlayer &&  !player.isDead)
            {
                player.Die();
            }
        }
        void CheckRayCasts()
        {
         //   Debug.DrawRay(transform.position, Vector2.right, Color.green, distance);
            RaycastHit2D hitPlayerInFront;  
            RaycastHit2D rightHit;      
            if (platform.isFacingRight)
            {
                hitPlayerInFront = Physics2D.Raycast(transform.position, Vector2.right, distanceToChase, playerLayer);
                rightHit = Physics2D.Raycast(transform.position, Vector2.right, distanceToKill, playerLayer);
            }
            else
            {
               hitPlayerInFront = Physics2D.Raycast(transform.position, -Vector2.right, distanceToChase, playerLayer);
                rightHit = Physics2D.Raycast(transform.position, -Vector2.right, distanceToKill, playerLayer);
            }
          
            if (hitPlayerInFront.collider != null)
            {
                isHittingPlayer = true;
            }
            else
            {
                isHittingPlayer = false;
            }
            if (!isBallEnemy)
            {
                if (rightHit.collider != null)
                {
                    player = rightHit.collider.gameObject.GetComponent<PlayerDie>();
                    canKillPlayer = true;
                }
                else
                {
                    canKillPlayer = false;
                }
            }


        }

        //this doesnt work
        //   HitRaycastSetBool(rightHit, canKillPlayer);
        //      HitRaycastSetBool(bottomHit, hitBelow);
        void HitRaycastSetBool(RaycastHit2D hit, bool boolToSet)
        {
            if(hit.collider != null)
            {
                boolToSet = true;
            }
            else
            {
                boolToSet = false;
            }
        }
        void Flip(bool isFacingRight)
        {
            if (!isFacingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);

            }
        }
        void CheckIsPlayerInViewLine()
        {
            //  isHittingPlayer = Physics2D.OverlapBox(frontCheck.position, cubeSize, 0, playerLayer);
        }
        private void OnDrawGizmos()
        {
            /*   [SerializeField] Vector2 cubeSize;
               [SerializeField] Vector2 bottomCubeSize;
                [SerializeField] float rightPositionExtra;
               Gizmos.color = Color.white;
               Gizmos.DrawWireCube(frontCheck.position, cubeSize);
               Gizmos.color = Color.red;
               Gizmos.DrawWireCube(bottomCheck.position, bottomCubeSize);
            
             
        [SerializeField] Transform frontCheck;
        [SerializeField] Transform bottomCheck;*/
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (isBallEnemy)
                {
                    player = collision.gameObject.GetComponent<PlayerDie>();
                    canKillPlayer = true;
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (isBallEnemy)
                {
                    canKillPlayer = false;
                }
            }
        }

    }

}