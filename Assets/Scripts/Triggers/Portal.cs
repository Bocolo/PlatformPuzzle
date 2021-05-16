using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Trigger
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] Portal otherPortal;
        [SerializeField] bool isTeleporting;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && isTeleporting == false)
            {
                isTeleporting = true;
                otherPortal.isTeleporting = true;
                collision.transform.position = otherPortal.transform.position;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && isTeleporting == true)
            {
                isTeleporting = false ;
         
            }
        }
    }
}
