using Resources.Scripts.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.Model
{
    public class Chaser : MonoBehaviour
    {
        private ChaserState chaserState;
        private GameObject chaserGameObject;
        private float rayLength;
        private Vector3 targetPos;
        private float movingSpeed;

        public void Initialize(GameObject chaser, float rayLength, float movingSpeed)
        {
            chaserGameObject = chaser;
            this.rayLength = rayLength;
            this.movingSpeed = movingSpeed;
            targetPos = chaserGameObject.transform.position;
            chaserState = ChaserState.Idle;
        }

        public void CheckSurroundingRays(GameObject player)
        {
            Ray forwardRay = new Ray();
            Ray leftRay = new Ray();
            Ray backRay = new Ray();
            Ray rightRay = new Ray();
            for (int i = 0; i < 4; i++)
            {
                Vector3 chaserPos = chaserGameObject.transform.position;
                Vector3 chaserSize = chaserGameObject.GetComponent<Collider>().bounds.size;
                Vector3 startPos;
                Vector3 direction;
                switch (i)
                {
                    case 0:
                        {
                            startPos = new Vector3(chaserPos.x, chaserPos.y, chaserPos.z + chaserSize.z / 2);
                            direction = Vector3.forward;
                            forwardRay = new Ray(startPos, direction);
                            break;
                        }
                    case 1:
                        {
                            startPos = new Vector3(chaserPos.x + chaserSize.x/2, chaserPos.y, chaserPos.z);
                            direction = Vector3.right;
                            rightRay = new Ray(startPos, direction);
                            break;
                        }
                    case 2:
                        {
                            startPos = new Vector3(chaserPos.x, chaserPos.y, chaserPos.z - chaserSize.z / 2);
                            direction = Vector3.back;
                            backRay = new Ray(startPos, direction);
                            break;
                        }
                    case 3:
                        {
                            startPos = new Vector3(chaserPos.x - chaserSize.x/2, chaserPos.y, chaserPos.z);
                            direction = Vector3.left;
                            leftRay = new Ray(startPos, direction);
                            break;
                        }
                    default:
                        {
                            startPos = Vector3.zero;
                            direction = Vector3.zero;
                            break;
                        }
                }
                Debug.DrawLine(startPos, startPos + direction * rayLength, Color.magenta);
            }
            if (Physics.Raycast(forwardRay, out RaycastHit raycastHit, rayLength))
            {
                if (raycastHit.collider.gameObject.Equals(player))
                {
                    MoveTowards(chaserGameObject.transform.position + Vector3.forward);
                }
            }
            if (Physics.Raycast(rightRay, out raycastHit, rayLength))
            {
                if (raycastHit.collider.gameObject.Equals(player))
                {
                    MoveTowards(chaserGameObject.transform.position + Vector3.right);
                }
            }
            if (Physics.Raycast(backRay, out raycastHit, rayLength))
            {
                if (raycastHit.collider.gameObject.Equals(player))
                {
                    MoveTowards(chaserGameObject.transform.position + Vector3.back);
                }
            }
            if (Physics.Raycast(leftRay, out raycastHit, rayLength))
            {
                if (raycastHit.collider.gameObject.Equals(player))
                {
                    MoveTowards(chaserGameObject.transform.position + Vector3.left);
                }
            }
        }

        private void MoveTowards(Vector3 pos)
        {
            if (chaserState is ChaserState.Idle or ChaserState.Moving)
            {
                if (chaserState == ChaserState.Idle)
                { // 移动之前检查当前位置以及目标位置是否可以前往
                    if (!chaserGameObject.transform.position.Equals(pos))
                    { // 当前位置不与目标位置重合
                        if (Ground.Accessible(pos) && Ground.Accessible(chaserGameObject.transform.position))
                        { // 目标地点和当前地点无障碍
                            targetPos = pos;
                            chaserGameObject.transform.position = Vector3.MoveTowards(
                                chaserGameObject.transform.position,
                                targetPos, Time.deltaTime * movingSpeed);
                            chaserState = ChaserState.Moving;
                        }
                    }
                }
                else
                { // 移动中
                    if (!chaserGameObject.transform.position.Equals(targetPos))
                    { // 还没有抵达目的地
                        chaserGameObject.transform.position = Vector3.MoveTowards(chaserGameObject.transform.position,
                            targetPos, Time.deltaTime * movingSpeed);
                    }
                    else
                    { // 到达目的地
                        chaserState = ChaserState.Idle;
                    }
                }
            }
        }

        public void Move()
        {
            MoveTowards(targetPos);
        }
        
        public IEnumerator BlinkAndDestroy(float blinkDuration, float cycle)
        {
            bool state = true;
            float elapse = 0;
            chaserState = ChaserState.Dead;
            chaserGameObject.GetComponent<Collider>().enabled = false;
            while (elapse < blinkDuration)
            {
                chaserGameObject.GetComponent<Renderer>().enabled = !state;
                state = !state;
                elapse += cycle / 2;
                yield return new WaitForSeconds(cycle/2);
            }
            chaserGameObject.GetComponent<Renderer>().enabled = !state;
            chaserGameObject.SetActive(false);
            yield return null;
        }
    }
}
