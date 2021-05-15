using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Platforms
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] float positionToMoveToX;
        [SerializeField] Vector2 startPosition;
        [SerializeField] Transform[] wayPoints;
        public float speed = 2f;
        [SerializeField] int currentWayPoint = 0;
        public bool isFacingRight = true;
        
     
        private void FixedUpdate()
        {


            if (transform.position != wayPoints[currentWayPoint].transform.position)
            {

                transform.position = Vector2.MoveTowards(transform.position,
                    wayPoints[currentWayPoint].transform.position,
                    speed * Time.deltaTime);
            }

                SetCurrentPoint();

            if (transform.position.x < wayPoints[currentWayPoint].transform.position.x)
            {
                isFacingRight = true;
            }
            else { isFacingRight = false; }
           
        }
            
        void SetCurrentPoint()
        {
            if(transform.position == wayPoints[currentWayPoint].transform.position)
            {
                currentWayPoint += 1;
            }
            if(currentWayPoint >= wayPoints.Length)
            {
                currentWayPoint = 0;
            }
        }
     
    }
}
