using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Camera;

namespace Platformer.Player
{

    public class PlayerManager : MonoBehaviour 
    {
        private static PlayerManager _instance;
        public static PlayerManager Instance => _instance;
        public PlayerDie[] players;
        [SerializeField] CameraFollow cameraFollow;
        int previousPlayerPosition;
        int counter = 0;


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        
        private void Start()
        {
            cameraFollow.currentPlayer = players[counter];
            previousPlayerPosition = counter;
            DeactivateOtherPlayers();    
      
        }

        private void Update()
        {
           
            if (counter != previousPlayerPosition)
            {
                cameraFollow.currentPlayer = players[counter];
                previousPlayerPosition = counter;
                DeactivateOtherPlayers();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
             
                if (players.Length > counter + 1)
                {
               
                    counter++;
                }
                else
                {
                    counter = 0;
                }
            }

        }

        //NOTE Layer14 is "other player" Layer 11 is " player
        void DeactivateOtherPlayers()
        {
            for (int i = 0; i < players.Length ; i++)
            {
                if (i != counter)
                {
                    players[i].GetComponent<PlayerController>().isActivePlayer = false;
                    players[i].gameObject.layer = 14;
                }
                else
                {
                    players[counter].GetComponent<PlayerController>().isActivePlayer = true;
                    players[counter].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); 
                    players[counter].gameObject.layer = 11;
                }
    
            }
        }
    }
}
