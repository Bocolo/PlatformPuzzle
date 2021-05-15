using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer.Trigger
{

    public class DoorTrigger : TriggerableObject //MonoBehaviour
    {

        [SerializeField] Vector2 locationToMoveTo;
        [SerializeField] float speed;
        Vector3 originalLocation;
        [SerializeField]Lever thisObjectsTrigger;
        bool hasBeenTriggered;
        bool hasReachedTop;
        bool hasReachedBottom =true;
        private void Start()
        {
            originalLocation = transform.position;
        }
        private void Update()
        {
            if (thisObjectsTrigger.isTriggered && !hasBeenTriggered)
            {
               
                hasBeenTriggered = true;
            }
            if(!thisObjectsTrigger.isTriggered && hasBeenTriggered)
            {
              
                hasBeenTriggered = false;
            }
            if (hasBeenTriggered)
            {
                if (!hasReachedTop)
                {
                    ActivateByTrigger();
                }
            }
            else
            {
                if (!hasReachedBottom)
                {
                    DeActivateByTrigger();
                }
            }
        }
        public override void ActivateByTrigger()
        {
            hasReachedBottom = false;
            Vector3 newPosition = new Vector3(transform.position.x, locationToMoveTo.y, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, newPosition,
                 speed * Time.deltaTime);
            if(transform.position == newPosition)
            {
                hasReachedTop = true;
            }         
        }
        public override void DeActivateByTrigger()
        {
            hasReachedTop = false;
            transform.position = Vector2.MoveTowards(transform.position, originalLocation, speed * Time.deltaTime);
            if (transform.position == originalLocation)
            {
                hasReachedBottom = true;
            }
        }
    }
}
