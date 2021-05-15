using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Trigger
{
    public class Lever : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        Color originalColor;
        Color activeColor;
        [SerializeField] bool canTrigger;
        public bool isTriggered = false;
        [SerializeField] bool canBeTriggeredMultipleTimes = false;

      

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
            activeColor = Color.green;
            
        }
 
        void SetSpriteColor() {
            if (canBeTriggeredMultipleTimes)
            {
                if (spriteRenderer.color == originalColor)
                {
                    spriteRenderer.color = activeColor;
                    isTriggered = true;
          
                }
                else
                {
                    spriteRenderer.color = originalColor;
                    isTriggered = false;
    
                }
            }
            else
            {
                if (spriteRenderer.color == originalColor)
                {
                    spriteRenderer.color = activeColor;
                    isTriggered = true;
       
                }
            }
        }
        
 
        private void OnTriggerEnter2D(Collider2D collision)
        {

            SetSpriteColor();
         
        }
 
    }
}

/*       void ActivateGameObject()
        {
            if (doorTrigger != null)
            {
                doorTrigger.ActivateByTrigger();
            }
        }
        void DeActivateGameObject()
        {
            if (doorTrigger != null)
            {
                doorTrigger.DeActivateByTrigger();
            }
        }*/
