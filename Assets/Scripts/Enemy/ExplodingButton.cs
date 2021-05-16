using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Player;

namespace Platformer.Obstacles
{
    public class ExplodingButton : MonoBehaviour
    {
        [SerializeField] float timeTillExplosion;
        bool isTimerActive;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] float radius;
        CircleCollider2D circleCollider;
        SpriteRenderer spriteRenderer;
        [SerializeField] SpriteRenderer glowRenderer;
        [SerializeField] float colorAlpha = 0f;
        [SerializeField] float upperAlpha = 0.8f;
   //     [SerializeField] float alphaChangeTime;
        bool isAlphaIncreasing = true;
        float originalAlpha;

        private void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            glowRenderer.color = new Color(.77f, .17f, .17f, colorAlpha);
            originalAlpha = colorAlpha;
        }
        private void Update()
        {
            glowRenderer.color = new Color(.77f, .17f, .17f, colorAlpha);
      
            if (isTimerActive)
            {
                ActivateExplosion();
            }
        }
        void ActivateExplosion()
        {
          

                if (isAlphaIncreasing)
                {
                    colorAlpha += Time.deltaTime  ;
               
                }
                else
                {
                    colorAlpha -= Time.deltaTime ;
                }

            
            if(colorAlpha >= upperAlpha )
            {
                isAlphaIncreasing = false;
               // colorAlpha = originalAlpha;
            }
            if( colorAlpha < originalAlpha)
            {
                isAlphaIncreasing = true;

            }
            if (timeTillExplosion > 0)
            {
                timeTillExplosion -= Time.deltaTime;
         
            }
            if (timeTillExplosion <= 0)
            {
         

                Invoke("Explode", .1f);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,radius);
        }
        void Explode()
        {
          
            //animation
            gameObject.SetActive(false);
           
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !PlayerDie.isDead)
            {
             //   Debug.Log("Player has triggered bomb");
                isTimerActive = true;
                circleCollider.radius = radius;
             
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !PlayerDie.isDead)
            {
                if (timeTillExplosion <= 0)
                {
                 //   Debug.Log("OnTriggerSTAycircle");
                    PlayerDie player = collision.gameObject.GetComponent<PlayerDie>();
                    player.Die();
                }
            }

        }
    }
}

// Debug.Log(timeTillExplosion);
// if(Physics.SphereCast(transform.position,))
/*       if (PlayerDie.isDead && timeTillExplosion <= 0)
       {
           gameObject.SetActive(false);
       }*/
/*    Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
             if (hitCollider.gameObject.CompareTag("Player"))
             {
                 Debug.Log("player in circle");
                 PlayerDie player = hitCollider.gameObject.GetComponent<PlayerDie>();
                 player.Die();
             }*/
