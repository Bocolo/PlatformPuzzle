using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.DeadPlayer;
namespace Platformer.Player
{

    public class PlayerDie : MonoBehaviour
    {

        [SerializeField] Transform startPoint;
        [SerializeField] GameObject deadSprite;
        [SerializeField] float enableInputTime=2f;
        [SerializeField] bool isDeadResetAutomatic =false;
        public bool hasDeadSprite = true;
      //  [SerializeField] float resetDeadCounter =2f;
         GameObject deadSpriteClone = null;
        public  bool isDead = false;
        bool isInputEnabled = true;
        public bool hasDeathAnimation = true;
        SpriteRenderer playerSprite;
        SpriteRenderer deadSpriteRenderer;
        PlayerController playerController;
        Collider2D collider;
        Rigidbody2D rb;
      
        private void Start()
        {
           playerSprite = GetComponent<SpriteRenderer>();
           playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody2D>();

            collider = GetComponent<BoxCollider2D>();
        }
        private void Update()
        {
            ResetPosition();
         
        }
        public  void Die()
        {
            //Temporary fixes, Require animations etc
            //Temporary fixes, Require animations etc
            Debug.Log("Should Die :  + " + gameObject.name);
            if (hasDeadSprite)
            {
                DeadAnimation();
            }
            else
            {
                DisablePlayer();
            }
            isDead = true;
           
        }
        void EnableInput()
        {
            isInputEnabled = true;
           
        }
        void EnablePlayer()// (bool enablePlayer)
        {
         //   collider.enabled = true;
            playerController.enabled = true;
            playerSprite.enabled = true;
       
        }
        void DisablePlayer()
        {
            playerController.enabled = false;
            playerSprite.enabled = false;
          //  collider.enabled = false;
        }
        void DeadAnimation()
        {
       
            DisablePlayer();
            isInputEnabled = false;
            transform.parent = null;
            if (deadSpriteClone == null)
            {
                deadSpriteClone = Instantiate(deadSprite, new Vector3(transform.position.x, transform.position.y, -0.1f), transform.rotation);
                deadSpriteClone.name = "deadSpriteClone";
                deadSpriteRenderer = deadSpriteClone.GetComponent<SpriteRenderer>();
                                    
            }
            else
            {
                deadSpriteRenderer.enabled = false;
                deadSpriteClone.transform.position = transform.position;        
            }
  
            deadSpriteRenderer.enabled = true;
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
            if (hasDeadSprite)
            {
                EnablePlayer();
            }
            else
            {

                Invoke("EnablePlayer", .3f);

            }
        }
   
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Danger") && !isDead)
            {
          //      Debug.Log("Collision Die : " + gameObject.name);
                Die();
            }
       
        }
    }
}
