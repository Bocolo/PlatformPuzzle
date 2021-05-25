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
        public int breakTorque = 100;
        bool isConnectedDirectlyToPlatform = false;
        Rigidbody2D rb;
        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();

        }
        private void Update()
        {
            if (playerController.isActivePlayer)
            {
                if (fj != null)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                    Debug.Log("5th Condition.   REMOVING Fixed Joint between: bottom/cb; " + fj.connectedBody.name + ".  And : top/fj ;" + gameObject.name);


                }
            }
            if (fj == null && playerController.hasJoint)
            {
                playerController.hasJoint = false;
            }
            if(fj != null)
            {
                //Some falling control needed
                if (!fj.connectedBody.gameObject.GetComponent<PlayerController>().isOnGround && playerController.isOnGround)//&& !fj.connectedBody.gameObject.GetComponent<PlayerController>().isOnTopOfOtherPlayer)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                    Debug.Log("1st Condition.   REMOVING Fixed Joint between: bottom/cb; " + fj.connectedBody.name + ".  And : top/fj ;" + gameObject.name);
                }
                //This is testing -- if works remove above
                if (!fj.connectedBody.gameObject.GetComponent<PlayerController>().isOnGround && !fj.connectedBody.gameObject.GetComponent<PlayerController>().playerIsOnPlatform &&!fj.connectedBody.gameObject.GetComponent<PlayerController>().isOnTopOfOtherPlayer)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                    Debug.Log("2nd Condition.   REMOVING Fixed Joint between: bottom/cb; " + fj.connectedBody.name + ".  And : top/fj ;" + gameObject.name);

                }
                if (fj.connectedBody.gameObject.GetComponent<PlayerDie>().isDead)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                    Debug.Log("3rd Condition.   REMOVING Fixed Joint between: bottom/cb; " + fj.connectedBody.name + ".  And : top/fj ;" + gameObject.name);

                }
                if (!playerController.isOnTopOfOtherPlayer)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                    Debug.Log("4th Condition.   REMOVING Fixed Joint between: bottom/cb; " + fj.connectedBody.name + ".  And : top/fj ;" + gameObject.name);

                }
            }
   
          
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
          
            ContactPoint2D contact = collision.contacts[0];
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
                {

                    if (collision.gameObject.CompareTag("Player") && collision.transform.parent == null && playerController.isOnTopOfOtherPlayer && !playerController.isActivePlayer && !playerController.hasJoint && fj == null)
                        {
                            playerController.hasJoint = true;
                            fj = gameObject.AddComponent<FixedJoint2D>();
                            fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                            fj.enableCollision = true;
                            fj.breakForce = breakForce;
                    Debug.Log("Adding Fixed Joint between: bottom/cb; " + collision.gameObject.name + ".  And : top/fj ;" + gameObject.name);
                        }                 
                    if (collision.gameObject.CompareTag("Player") && collision.transform.parent != null)
                {
                    transform.parent = collision.transform.parent;
                    Debug.Log("Setting as Child through player contact");
                }
                }
          
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && transform.parent != null &&!playerController.playerIsOnPlatform)// && playerController.isOnTopOfOtherPlayer)
            {
                transform.parent = null;
                Debug.Log("removing player/platform parent");
            }
        }
    }
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