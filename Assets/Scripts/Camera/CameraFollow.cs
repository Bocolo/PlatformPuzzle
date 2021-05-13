using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] GameObject objectToFollow;

        [SerializeField] float speed = 2.0f;
        [SerializeField] float xOffset = 0;
        [SerializeField] float yOffset = 0;

        void Update()
        {
            float interpolation = speed * Time.deltaTime;

            Vector3 position = this.transform.position;
            position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y +yOffset, interpolation);
            position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x + xOffset, interpolation);

            this.transform.position = position;
        }
    }
}
