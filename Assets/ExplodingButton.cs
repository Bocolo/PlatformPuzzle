using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Player;

namespace Platformer.Obstacles
{
    public class ExplodingButton : MonoBehaviour
    {
        [SerializeField] float timeTillExplosion;
        [SerializeField] bool isTimerActive;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] float radius;
        CircleCollider2D circleCollider;
        SpriteRenderer spriteRenderer;

        private void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            if (isTimerActive)
            {
                ActivateExplosion();
            }
        }
        void ActivateExplosion()
        {
            //Trigger timer for seconds
            //sphere casts 
            //explode
            //kill player if in sphere
            if (timeTillExplosion > 0)
            {
                timeTillExplosion -= Time.deltaTime;
               // Debug.Log(timeTillExplosion);
            }
            if(timeTillExplosion <= 0)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
                if (hitCollider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("player in circle");
                    PlayerDie player = hitCollider.gameObject.GetComponent<PlayerDie>();
                    player.Die();
                }
              
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
            // if(Physics.SphereCast(transform.position,))
            //animation

          
            gameObject.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !PlayerDie.isDead)
            {
                Debug.Log("Player has triggered bomb");
                isTimerActive = true;
                circleCollider.radius = radius;
             
            }
        }
       /* private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !PlayerDie.isDead)
            {
                if (timeTillExplosion <= 0)
                {
                    Debug.Log("OnTriggerSTAycircle");
                    PlayerDie player = collision.gameObject.GetComponent<PlayerDie>();
                    player.Die();
                }
            }
         *//*   if (PlayerDie.isDead  && timeTillExplosion<=0) {
                gameObject.SetActive(false);
            }*//*
        }*/
    }
}
