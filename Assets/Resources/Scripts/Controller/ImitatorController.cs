using Resources.Scripts.Enum;
using Resources.Scripts.Model;
using System;
using UnityEngine;

namespace Resources.Scripts.Controller
{
	public class ImitatorController : MonoBehaviour
	{
		public GameObject patroller;
		public float movingSpeed;

		private Imitator imitator;
		private PlayerController player;

		private void Start()
		{
			if (movingSpeed == 0) { movingSpeed = 2; }
			imitator = gameObject.AddComponent<Imitator>();
			imitator.Initialize(gameObject,
				movingSpeed);
			player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		}

		private void Update()
		{
			imitator.ImitatePlayerMove(player);
			Paint();
		}

		private void Paint()
		{
			if (imitator.ImitatorState == ImitatorState.Idle)
			{
				Ground.Paint(imitator.ImitatorGameObject.transform.position);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (patroller && other.gameObject.Equals(patroller))
			{
				enabled = false;
				StartCoroutine(imitator.BlinkAndDestroy(2, 0.5f));
			}
		}
	}
}
