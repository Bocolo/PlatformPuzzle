using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerJoint : MonoBehaviour
    {
        PlayerController playerController;
        FixedJoint2D fj;
      //  bool hasJoint = false;
        int breakForce = 100;
        private void Start()
        {
            playerController = GetComponent<PlayerController>();

        }
        private void Update()
        {
            if (playerController.isActivePlayer)
            {
                if (fj != null)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                }
            }
            if (fj == null && playerController.hasJoint)
            {
                playerController.hasJoint = false;
            }
        }



        private void OnCollisionStay2D(Collision2D collision)
        {
        
            if (collision.gameObject.CompareTag("Player") &&  playerController.isOnTopOfOtherPlayer && !playerController.isActivePlayer && !playerController.hasJoint)
            {

                playerController.hasJoint = true;
                /*   fj = collision.gameObject.AddComponent<FixedJoint2D>();
                   fj.connectedBody = rb;
   */
                fj = gameObject.AddComponent<FixedJoint2D>();
                fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                fj.enableCollision = true;
                fj.breakForce = breakForce;

            }
        }
    }
}
