using Resources.Scripts.Model;
using System;
using UnityEngine;

namespace Resources.Scripts.Controller
{
	public class PatrollerController : MonoBehaviour
	{
		public GameObject imitator;
		public int[] patrolRoute;
		public float movingSpeed;

		private Patroller patroller;

		private void Start()
		{
			if (patrolRoute.Length == 0) { patrolRoute = new[] { 0 }; }
			if (movingSpeed == 0) { movingSpeed = 1; }
			patroller = gameObject.AddComponent<Patroller>();
			patroller.Initialize(gameObject, movingSpeed, patrolRoute);
		}

		private void Update()
		{
			patroller.Patrol();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (imitator && other.gameObject.Equals(imitator) ||
			    other.gameObject.Equals(GameObject.Find("Bullet")))
			{
				StartCoroutine(patroller.BlinkAndDestroy(2, 0.5f));
			}
		}

	}
}
