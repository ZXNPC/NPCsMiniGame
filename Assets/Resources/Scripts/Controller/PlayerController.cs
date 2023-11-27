using Resources.Scripts.Enum;
using Resources.Scripts.Model;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Resources.Scripts.Controller
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float movingSpeed = 2;
		[FormerlySerializedAs("bullet")]
		[SerializeField]
		private GameObject bulletPrefab;

		public PlayerState playerState = PlayerState.Idle;
		public Vector3 targetPosition;
		private bool ableToShoot = false;

		private void Start()
		{
			targetPosition = transform.position;
		}

		private void Update()
		{
			Move();
			ReachGoal();
			Paint();
			AcquireBulletItem();
			Shoot();
		}

		private void Move()
		{
			if(playerState == PlayerState.Idle)
			{
				if(Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") != 0)
				{
					targetPosition = transform.position + Vector3.forward * Input.GetAxisRaw("Vertical");
				}
				else if(Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)
				{
					targetPosition = transform.position + Vector3.right * Input.GetAxisRaw("Horizontal");
				}
			}

			MoveTowards(targetPosition);
		}

		private void MoveTowards(Vector3 pos)
		{
			if(playerState == PlayerState.Idle || playerState == PlayerState.Moving)
			{
				if(playerState == PlayerState.Idle)
				{
					if(Ground.Accessible(pos) && Ground.Accessible(transform.position))
					{ // 目标地点和当前地点无障碍
						targetPosition = pos;
						transform.position = Vector3.MoveTowards(
							transform.position,
							targetPosition, Time.deltaTime * movingSpeed);
						playerState = PlayerState.Moving;
					}
				}
				else
				{ // 移动中
					if(!transform.position.Equals(targetPosition))
					{ // 还没有抵达目的地
						transform.position = Vector3.MoveTowards(transform.position,
							targetPosition, Time.deltaTime * movingSpeed);
					}
					else
					{ // 到达目的地
						playerState = PlayerState.Idle;
					}
				}
			}

		}

		private void ReachGoal()
		{
			if(playerState == PlayerState.Idle)
			{
				Vector3 playerPosition = transform.position;
				playerPosition.y = 0;
				Vector3 goalPosition = GameObject.FindWithTag("Finish").transform.position;
				goalPosition.y = 0;

				if(playerPosition == goalPosition)
				{
					Debug.Log("Reach goal!");
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
			}
		}

		private void Paint()
		{
			if(playerState == PlayerState.Idle)
			{
				Ground.Paint(transform.position);
			}
		}

		public void AcquireShootAbility()
		{
			GetComponent<Animator>().SetBool("isDeforming", true);
			ableToShoot = true;
		}

		private void AcquireBulletItem()
		{
			GameObject bulletItem = GameObject.FindWithTag("ShootAbility");
			if(playerState == PlayerState.Idle && bulletItem)
			{
				Vector3 bulletItemPosition = bulletItem.transform.position;
				bulletItemPosition.y = 0;
				Vector3 playerPosition = transform.position;
				playerPosition.y = 0;
				if(bulletItemPosition == playerPosition)
				{
					AcquireShootAbility();
					Destroy(bulletItem);
				}
			}
		}

		public void Shoot(Vector3 pos, string bulletPrefabPath = "Prefabs/Bullet")
		{
			if(ableToShoot)
			{ // 可以发射子弹
				if(!transform.position.Equals(pos))
				{ // 子弹目标位置不等于玩家当前位置
					if(Ground.Accessible(pos) && Ground.Accessible(transform.position) && !GameObject.Find("Bullet"))
					{ // 发射之前检查目标位置是否可以发射子弹
						// 生成子弹
						Bullet bullet = Instantiate(bulletPrefab, null).GetComponent<Bullet>();
						bullet.Initialize(transform.position, pos);
					}
				}
			}
		}

		private void Shoot()
		{
			Vector3 targetPos;
			if(playerState == PlayerState.Idle)
			{
				if(Input.GetKey(KeyCode.I))
				{
					targetPos = transform.position + Vector3.forward;
				}
				else if(Input.GetKey(KeyCode.K))
				{
					targetPos = transform.position + Vector3.back;
				}
				else if(Input.GetKey(KeyCode.J))
				{
					targetPos = transform.position + Vector3.left;
				}
				else if(Input.GetKey(KeyCode.L))
				{
					targetPos = transform.position + Vector3.right;
				}
				else
				{
					return;
				}

				Shoot(targetPos);
			}
		}
		
		public IEnumerator BlinkAndRestart(float blinkDuration, float cycle)
		{
			bool state = true;
			float elapse = 0;
			playerState = PlayerState.Dead;
			while(elapse < blinkDuration)
			{
				GetComponent<Renderer>().enabled = !state;
				state = !state;
				elapse += cycle / 2;
				yield return new WaitForSeconds(cycle / 2);
			}
			GetComponent<Renderer>().enabled = !state;
			yield return null;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnTriggerEnter(Collider other)
		{
			GameObject patroller = GameObject.FindWithTag("Patroller");
			GameObject imitator = GameObject.FindWithTag("Imitator");
			GameObject chaser = GameObject.FindWithTag("Chaser");
			if(patroller && other.gameObject.Equals(patroller) || imitator && other.gameObject.Equals(imitator) ||
			   chaser && other.gameObject.Equals(chaser))
			{
				StartCoroutine(BlinkAndRestart(3, 0.5f));
			}
		}
	}
}
