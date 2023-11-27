using Resources.Scripts.Model;
using System;
using UnityEngine;

namespace Resources.Scripts.Controller
{
    public class GroundController : MonoBehaviour
    {
        public float changeCycle;
        private void Start()
        {
            Ground.Initialize(gameObject);
            ChangeByCycle();
        }

        private void Update()
        {
        }

        private void ChangeByCycle()
        {
            if (changeCycle > 0)
            {
                StartCoroutine(Ground.Change(2));
            }
        }
    }
}
