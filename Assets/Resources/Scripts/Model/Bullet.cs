using Resources.Scripts.Enum;
using System;
using UnityEngine;

namespace Resources.Scripts.Model
{
    public class Bullet : MonoBehaviour
    {
        private float movingSpeed;
        private Vector3 targetPos;

        private void Update()
        {
            // 应该在上一层就校验完成了，所以直接运行
            if (!transform.position.Equals(targetPos))
            { // 移动中
                transform.position = Vector3.MoveTowards(transform.position,
                    targetPos,
                    Time.deltaTime * movingSpeed);
            }
            else
            { // 到达目的地
                Debug.Log("Bullet destroyed!");
                Destroy(gameObject);
            }
        }

        public void Initialize(Vector3 spawnPosition, Vector3 targerPos, float movingSpeed = 2)
        {
            this.movingSpeed = movingSpeed;
            this.targetPos = targerPos;

            Vector3 playerPosition = spawnPosition;
            transform.position = playerPosition;
            transform.forward = Vector3.Normalize(this.targetPos - playerPosition);
        }

        public Vector3 TargetPos
        {
            get { return targetPos; }
            set { targetPos = value; }
        }
    }
}
