using Resources.Scripts.Enum;
using Resources.Scripts.Model;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Scripts.Controller
{
    public class ChaserController : MonoBehaviour
    {
        public GameObject player;
        public float rayLength;
        public float movingSpeed;

        private Chaser chaser;

        private void Start()
        {
            if (movingSpeed <= 0) { movingSpeed = 2; }
            if (rayLength <= 0) { rayLength = 1; }
            chaser = gameObject.AddComponent<Chaser>();
            chaser.Initialize(gameObject, rayLength, movingSpeed);
        }

        private void Update()
        {
            chaser.CheckSurroundingRays(player);
            chaser.Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(chaser.BlinkAndDestroy(2, 0.5f));
        }
    }
}
