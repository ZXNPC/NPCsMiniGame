using Resources.Scripts.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Model
{
    public class Patroller : MonoBehaviour
    {
        private GameObject patrollerGameObject;
        private PatrollerState patrollerState;
        private List<Vector3> patrolRoute;
        private int nextPatrolIndex;
        private float movingSpeed;
        private bool isMoving;

        public void Initialize(GameObject patroller, float movingSpeed, int[] route)
        {
            patrollerGameObject = patroller;
            patrollerState = PatrollerState.Idle;
            this.movingSpeed = movingSpeed;
            isMoving = false;
            
            Vector3 startPos = new Vector3(Ground.MinX, patrollerGameObject.transform.position.y, Ground.MaxZ);

            // 按照从左到右，从上到下的顺序为地板编号
            List<Vector3> patrolPos = new List<Vector3>();
            for (int i = 0; i < Ground.Height / Ground.Interval; i++)
            {
                for (int j = 0; j < Ground.Width / Ground.Interval; j++)
                {
                    patrolPos.Add(startPos + Vector3.right * j * Ground.Interval + Vector3.back * i * Ground.Interval);
                }
            }

            patrolRoute = new List<Vector3>();
            foreach(int t in route)
            {
                int index = t - 1;
                patrolRoute.Add(index != -1 ? patrolPos[index] : patrollerGameObject.transform.position);
            }

            nextPatrolIndex = (patrolRoute.IndexOf(patrollerGameObject.transform.position) + 1) % patrolRoute.Count;
        }

        public void Patrol()
        {
            if (patrollerState != PatrollerState.Dead && patrolRoute.Count != 0)
            {
                if (!isMoving)
                { // 开始移动
                    if (Ground.Accessible(patrolRoute[nextPatrolIndex]) && Ground.Accessible(patrollerGameObject.transform.position))
                    {
                        isMoving = true;
                        patrollerGameObject.transform.position = Vector3.MoveTowards(patrollerGameObject.transform.position,
                            patrolRoute[nextPatrolIndex], Time.deltaTime * movingSpeed);
                        patrollerState = PatrollerState.Moving;
                    }
                }
                else
                { // 开始移动
                    if (!patrollerGameObject.transform.position.Equals(patrolRoute[nextPatrolIndex]))
                    { // 还没有抵达目的地
                        patrollerGameObject.transform.position = Vector3.MoveTowards(patrollerGameObject.transform.position,
                            patrolRoute[nextPatrolIndex], Time.deltaTime * movingSpeed);
                    }
                    else
                    { // 到达目的地
                        isMoving = false;
                        patrollerState = PatrollerState.Idle;
                        nextPatrolIndex = (nextPatrolIndex + 1) % patrolRoute.Count;
                    }
                }
            }
        }

        public IEnumerator BlinkAndDestroy(float blinkDuration, float cycle)
        {
            bool state = true;
            float elapse = 0;
            patrollerState = PatrollerState.Dead;
            patrollerGameObject.GetComponent<Collider>().enabled = false;
            while (elapse < blinkDuration)
            {
                patrollerGameObject.GetComponent<Renderer>().enabled = !state;
                state = !state;
                elapse += cycle / 2;
                yield return new WaitForSeconds(cycle/2);
            }
            patrollerGameObject.GetComponent<Renderer>().enabled = !state;
            patrollerGameObject.SetActive(false);
            yield return null;
        }
    }
}
