using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Obstacles
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] GameObject laser;
        [SerializeField] float timeToBeActive;
        [SerializeField] float offsetTime;
        float timeRemaining;
        bool isActive;
        private void Start()
        {
            timeRemaining = timeToBeActive + offsetTime;
        }

        private void Update()
        {
             if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            if (timeRemaining <=0)
            {
                if (isActive)
                {
                    SetDeactivated();
                    timeRemaining = timeToBeActive;
                }
                else
                {
                    SetActivated();
                    timeRemaining = timeToBeActive;
                }
            }
        }
        void SetActivated()
        {
            laser.SetActive(true);
            isActive = true;
  
        }
        void SetDeactivated()
        {
            laser.SetActive(false);
            isActive = false;
        }
    }
}
