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
                }
                if(!playerController.isOnTopOfOtherPlayer)
                {
                    Destroy(GetComponent<FixedJoint2D>());
                    playerController.hasJoint = false;
                }
            }
         /*   if(fj!= null)
            {
                if (fj.connectedBody.gameObject.transform.parent.CompareTag("Platform"))
                {
                    //  transform.parent = fj.connectedBody.gameObject.transform.parent.transform;
                    transform.parent = fj.connectedBody.gameObject.transform;
                }
               
            }*/
         /*   else if( fj ==null && transform.parent != null && !isConnectedDirectlyToPlatform)
            {
                
                    transform.parent = null;
                
            }*/
          
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
         
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
      /*      if (transform.parent != null)
            {
                if (collision.gameObject.CompareTag("Platform") && transform.parent == collision.gameObject.transform)
                {
                    isConnectedDirectlyToPlatform = false;
                    transform.parent = null;
                }
                if (collision.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Pass 1");
                    if (transform.parent == collision.gameObject.transform.parent.transform)
                    {
                        Debug.Log("Pass 2");
                        
                            transform.parent = null;
                        
                    } 
                }
               
            }*/
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
 /*           if (collision.contacts.Length > 0 && playerController.isOnTopOfOtherPlayer && !playerController.isOnGround)
            {

                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    if (Vector3.Dot(collision.contacts[i].normal, Vector3.up) > 0.5)
                    {
                        //   Debug.Log("cont on bottom top");

                        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.parent != null)// && collision.gameObject.transform.parent !=null)
                        {
                            transform.parent = collision.gameObject.transform.parent.transform;

                        }
                        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.parent == null)
                        {
                            transform.parent = null;
                        }
                        *//*   if(collision.gameObject.CompareTag("Player") && collision.gameObject.transform.parent == null)
                           {
                               transform.parent = null;
                           }*//*

                    }

                }
            }*/
            if (collision.contacts.Length > 0)
            {
               // ContactPoint2D contact = collision.contacts[0];
                for (int i = 0; i < collision.contacts.Length; i++)
                {
                    if (Vector3.Dot(collision.contacts[i].normal, Vector3.up) > 0.5)
                    {
                     /*   if (collision.gameObject.CompareTag("Platform") && transform.parent == null)
                        {
                            isConnectedDirectlyToPlatform = true;
                            transform.parent = collision.transform;
                        }*/
                        if (collision.gameObject.CompareTag("Player") && playerController.isOnTopOfOtherPlayer && !playerController.isActivePlayer && !playerController.hasJoint && fj == null)
                        {

                            playerController.hasJoint = true;

                            fj = gameObject.AddComponent<FixedJoint2D>();
                            fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                            fj.enableCollision = true;
                            fj.breakForce = breakForce;
                 //           fj.breakTorque = breakTorque;
                            Debug.Log("Testing joint on Collision " + gameObject.name);
                        }
                  /*      if (collision.gameObject.CompareTag("Platform") && !playerController.hasJoint && fj == null)
                        {
                            Debug.Log("onPlatform");
                            playerController.hasJoint = true;

                            fj = gameObject.AddComponent<FixedJoint2D>();
                            fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                            fj.enableCollision = true;
                            fj.breakForce = breakForce;
                        }*/

                    }

                }
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