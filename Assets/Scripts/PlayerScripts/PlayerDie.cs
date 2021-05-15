using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{

    public class PlayerDie : MonoBehaviour
    {

        [SerializeField] Transform startPoint;
        [SerializeField] GameObject deadSprite;
        [SerializeField] float enableInputTime=2f;
         GameObject deadSpriteClone = null;
        public static bool isDead = false;
        bool isInputEnabled = true;
        SpriteRenderer player;
        SpriteRenderer deadSpriteRenderer;
        PlayerController playerController;
       // GameObject deadSpriteClone =null;
        List<GameObject> cloneList = new List<GameObject>();
        private void Start()
        {
           player = GetComponent<SpriteRenderer>();
           playerController = GetComponent<PlayerController>();

        }
        private void Update()
        {
            ResetPosition();
        }
        public  void Die()
        {
            Debug.Log("YouDied");
            //Leave body copy behind
            //on push 
            DeadAnimation();
            isDead = true;
           
        }
        void EnableInput()
        {
            isInputEnabled = true;
           
        }
        void DeadAnimation()
        {
            playerController.enabled = false;
            isInputEnabled = false;
            if (deadSpriteClone == null)
            {
                deadSpriteClone = Instantiate(deadSprite, new Vector3(transform.position.x, transform.position.y, -0.1f), transform.rotation);
                deadSpriteClone.name = "deadSpriteClone";
                deadSpriteRenderer = deadSpriteClone.GetComponent<SpriteRenderer>();
                Debug.Log(transform.position);
                player.enabled = false;
                deadSpriteRenderer.enabled = true;
               
                
            }
            else
            {
                deadSpriteRenderer.enabled = false;
                deadSpriteClone.transform.position = transform.position;
                player.enabled = false;
                deadSpriteRenderer.enabled = true;
                Debug.Log("2: " +transform.position);
            }
            //Need to Disable player movement
            transform.position = startPoint.position;
            Invoke("EnableInput", enableInputTime);
        }
     //   invoke(ResetPosition, 2f);
        void ResetPosition()
        {
            if (isInputEnabled)
            {
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) && isDead)
                {
                    Debug.Log("You are Resetting Position");
                    //Reset to starting Transform 
                    isDead = false;
             
                    player.enabled = true;
                    playerController.enabled = true;

                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Danger") && !isDead)
            {
                Die();
            }
        }
    }
}
