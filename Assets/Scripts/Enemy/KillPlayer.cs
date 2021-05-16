using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Player;

namespace Platformer.Obstacles
{
    public class KillPlayer : MonoBehaviour
    {

       // PlayerDie player;
        void Start()
        {
         //   player = FindObjectOfType<PlayerDie>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player")&& !collision.gameObject.GetComponent<PlayerDie>().isDead)
            {
                PlayerDie player = collision.gameObject.GetComponent<PlayerDie>();
                player.Die();
            }
        }
    }

}
//& collision.gameObject.GetComponent<PlayerDie>().isDead &&