using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Player;

namespace Platformer.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] GameObject objectToFollow;

        [SerializeField] float speed = 2.0f;
        [SerializeField] float xOffset = 0;
        [SerializeField] float yOffset = 0;
        GameObject deadTarget;
        [SerializeField] PlayerDie[] players;
        public PlayerDie currentPlayer;
        int numberOfPlayers;
        int previousPlayerPosition;
        int counter = 0;
        
        private void Start()
        {
        /*    currentPlayer = players[counter];
            previousPlayerPosition = counter;
            numberOfPlayers = players.Length;*/
        }

        void Update()
        {
            float interpolation = speed * Time.deltaTime;
            Vector3 position = this.transform.position;
/*            if (counter != previousPlayerPosition)
            {
                currentPlayer = players[counter];
                previousPlayerPosition = counter;
                Debug.Log("Updating camera positin with active player");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
              //  Debug.Log(players.Length);
               // Debug.Log(numberOfPlayers);
                if (players.Length > counter + 1) 
                {
                    // currentPlayer = players[counter + 1];
                    counter++;
                }
                else
                {
                    counter = 0;
                }
            }*/
            if (currentPlayer.isDead && currentPlayer.hasDeadSprite)
            {
                SetTargetToDead();
                position.y = Mathf.Lerp(this.transform.position.y, deadTarget.transform.position.y + yOffset, interpolation);
                position.x = Mathf.Lerp(this.transform.position.x, deadTarget.transform.position.x + xOffset, interpolation);
            }

            else
            {
                /* position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + yOffset, interpolation);
                 position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x + xOffset, interpolation);*/
                position.y = Mathf.Lerp(this.transform.position.y, currentPlayer.transform.position.y + yOffset, interpolation);
                position.x = Mathf.Lerp(this.transform.position.x, currentPlayer.transform.position.x + xOffset, interpolation);
            }
            this.transform.position = position;
        }
        bool SetTargetToDead()
        {
            //will need to update this for multi dead targets 
            if (deadTarget == null)
            {
                deadTarget = GameObject.FindGameObjectWithTag("DeadPlayer");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
