using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Trigger
{
    public class TriggerableObject : MonoBehaviour
    {
        public virtual void ActivateByTrigger()
        {
            Debug.Log("Activating Trigger at Base");
        }
        public virtual void DeActivateByTrigger()
        {
            Debug.Log("DeActivating Trigger at Base");
        }
    }
}
