using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer.Trigger
{

    public class DoorTrigger : TriggerableObject //MonoBehaviour
    {

        [SerializeField]Lever thisObjectsTrigger;
        [SerializeField] bool hasBeenTriggered;
        private void Update()
        {
            if (thisObjectsTrigger.isTriggered && !hasBeenTriggered)
            {
                ActivateByTrigger();
                hasBeenTriggered = true;
            }
            if(!thisObjectsTrigger.isTriggered && hasBeenTriggered)
            {
                DeActivateByTrigger();
                hasBeenTriggered = false;
            }
        }
        public override void ActivateByTrigger()
        {
          //  base.ActivateByTrigger();
            Debug.Log("Activating Door");
        }
        public override void DeActivateByTrigger()
        {
            Debug.Log("DeActivating Door");
          //  base.DeActivateByTrigger();
        }
    }
}
