using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Camera;

namespace Platformer.Player
{
 //   public class PlayerManager<T> : MonoBehaviour where T: MonoBehaviour
    public class PlayerManager : MonoBehaviour 
    {
        private static PlayerManager _instance;
        public static PlayerManager Instance => _instance;
        // Camera mainCamera;
        public PlayerDie[] players;
        [SerializeField] CameraFollow cameraFollow;

        PlayerDie currentPlayer;
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
         //   numberOfPlayers = players.Length;
        }
        private void Update()
        {
            if (counter != previousPlayerPosition)
            {
                cameraFollow.currentPlayer = players[counter];
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
            }
        }
    }
}
