using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerShooter : MonoBehaviour
    {

        [SerializeField] bool canShoot;
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform shootPoint;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Shoot();
            }
        }
        void Shoot()
        {
            Debug.Log("Player is Shooting");
        }
    }

}