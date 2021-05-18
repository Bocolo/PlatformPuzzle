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
        [SerializeField] bool isDeadResetAutomatic =false;
      //  [SerializeField] float resetDeadCounter =2f;
         GameObject deadSpriteClone = null;
        public  bool isDead = false;
        bool isInputEnabled = true;
         bool canPlayerMove =true;
        public bool hasDeathAnimation = true;
        SpriteRenderer player;
        SpriteRenderer deadSpriteRenderer;
        PlayerController playerController;
        
        Rigidbody2D rb;
      
        private void Start()
        {
           player = GetComponent<SpriteRenderer>();
           playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
           
        }
        private void Update()
        {
            ResetPosition();
         
        }
        public  void Die()
        {
        
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
            transform.parent = null;
            if (deadSpriteClone == null)
            {
                deadSpriteClone = Instantiate(deadSprite, new Vector3(transform.position.x, transform.position.y, -0.1f), transform.rotation);
                deadSpriteClone.name = "deadSpriteClone";
                deadSpriteRenderer = deadSpriteClone.GetComponent<SpriteRenderer>();
                player.enabled = false;
                deadSpriteRenderer.enabled = true;                              
            }
            else
            {
                deadSpriteRenderer.enabled = false;
                deadSpriteClone.transform.position = transform.position;
                player.enabled = false;
                deadSpriteRenderer.enabled = true;
            }

            //   FreezeContraints();

                Invoke("ResetTransform", 0.5f);
            Invoke("EnableInput", enableInputTime);
        }
   
        void ResetTransform()
        {
            transform.position = startPoint.position;
        }
        void ResetPosition()
        {
            if (isInputEnabled)
            {
                if (!isDeadResetAutomatic)
                {
                    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) && isDead)
                    {
                        ResetAfterDeath();
                    }
                }
                else if(isDead)
                {
                    ResetAfterDeath();
                }
            }
        }
        void ResetAfterDeath()
        {
            isDead = false;
            ResetTransform();

            player.enabled = true;
            playerController.enabled = true;
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
