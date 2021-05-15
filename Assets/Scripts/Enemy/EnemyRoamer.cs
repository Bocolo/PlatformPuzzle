using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Platforms;
using Platformer.Player;

namespace Platformer.Enemy
{
    public class EnemyRoamer : MonoBehaviour
    {
        [SerializeField] float rightPositionExtra;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] Vector2 cubeSize;
        [SerializeField] Vector2 bottomCubeSize;
        [SerializeField] float speedUp;
        [SerializeField] float originalSpeed;
        [SerializeField] Transform frontCheck;
        [SerializeField] Transform bottomCheck;
        [SerializeField] bool isHittingPlayer;
        [SerializeField] bool canKillPlayer;
        PlatformMover platform;
        PlayerDie player;

        private void Start()
        {
            platform = GetComponent<PlatformMover>();
            player = FindObjectOfType<PlayerDie>();
            originalSpeed = platform.speed;
        }
        private void Update()
        {
             CheckIsPlayerInViewLine();
            CheckWillPlayerBeKilled();
            ChasePlayer();
        }
        private void FixedUpdate()
        {
     
            Flip(platform.isFacingRight);
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
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(frontCheck.position, cubeSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bottomCheck.position, bottomCubeSize);
        }
        void CheckIsPlayerInViewLine()
        {
            isHittingPlayer = Physics2D.OverlapBox(frontCheck.position, cubeSize, 0, playerLayer);
        }
        void CheckWillPlayerBeKilled()
        {
            canKillPlayer = Physics2D.OverlapBox(bottomCheck.position, bottomCubeSize, 0, playerLayer);
            if (canKillPlayer)
            {
                player.Die();
            }
        }
        void CheckRayCasts()
        {
          
       /*     RaycastHit2D hitRight = Physics2D.Raycast(transform.position,
                transform.right, rightPositionExtra, playerLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position,
                -transform.right, rightPositionExtra, playerLayer);
         
            if (hitRight.collider.gameObject.tag == "Player")
            {
                Debug.Log("hit player right");

            }

            if (hitLeft.collider.gameObject.tag == "Player")
            {
                Debug.Log("hit player left");

            }*/

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

    }

}