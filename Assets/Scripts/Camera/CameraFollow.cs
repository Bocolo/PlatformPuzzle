﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Player;

namespace Platformer.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] GameObject objectToFollow;

        [SerializeField] float speed = 2.0f;
        [SerializeField] float xOffset = 0;
        [SerializeField] float yOffset = 0;
        GameObject deadTarget;
        private void Start()
        {
        
        }

        void Update()
        {
            float interpolation = speed * Time.deltaTime;
            Vector3 position = this.transform.position;
            if (PlayerDie.isDead)
            {
                SetTargetToDead();
                Debug.Log("is dead from camera script");
                position.y = Mathf.Lerp(this.transform.position.y, deadTarget.transform.position.y + yOffset, interpolation);
                position.x = Mathf.Lerp(this.transform.position.x, deadTarget.transform.position.x + xOffset, interpolation);

           //     this.transform.position = position;
            }

            else
            {
              
                position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + yOffset, interpolation);
                position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x + xOffset, interpolation);

               
            }
            this.transform.position = position;
        }
        bool SetTargetToDead()
        {
            deadTarget = GameObject.FindGameObjectWithTag("DeadPlayer");
            if (deadTarget == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}