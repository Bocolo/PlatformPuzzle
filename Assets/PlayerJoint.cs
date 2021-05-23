using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerJoint : MonoBehaviour
    {
        PlayerController playerController;
        FixedJoint2D fj;
        RelativeJoint2D rj;
      //  bool hasJoint = false;
        public int breakForce = 100;
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
            if(fj != null)
            {
                if (!fj.connectedBody.gameObject.GetComponent<PlayerController>().isOnGround)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                }
            }
          
        }

        private void OnCollisionStay2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Player") && playerController.isOnTopOfOtherPlayer && !playerController.isActivePlayer && !playerController.hasJoint &&fj==null)
            {

                playerController.hasJoint = true;

                fj = gameObject.AddComponent<FixedJoint2D>();
                fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                fj.enableCollision = true;
                fj.breakForce = breakForce;
                Debug.Log("Testing joint on Collision");
            }
            /*   if (collision.gameObject.CompareTag("Platform") &&rj==null && playerController.playerIsOnPlatform && !playerController.isActivePlayer && !playerController.hasJoint)
               {
                   Debug.Log("is onPlatform");
                   playerController.hasJoint = true;
                   Debug.Log(playerController.hasJoint);
           *//*        fj = collision.gameObject.AddComponent<FixedJoint2D>();
                   fj.connectedBody = playerController.rb;
   *//*
                   rj = gameObject.AddComponent<RelativeJoint2D>();
                   rj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                   rj.enableCollision = true;
                  // fj.breakForce = breakForce;

               }*/
        }
    }
}
