using Resources.Scripts.Controller;
using Resources.Scripts.Enum;
using System.Collections;
using UnityEngine;

namespace Resources.Scripts.Model
{
    public class Imitator : MonoBehaviour
    {
        private ImitatorState imitatorState;
        private GameObject imitatorGameObject;
        private float movingSpeed;
        private bool isMoving;
        private Vector3 targetPos;

        public void Initialize(GameObject imitator, float movingSpeed)
        {
            imitatorGameObject = imitator;
            imitatorState = ImitatorState.Idle;
            this.movingSpeed = movingSpeed;
            isMoving = false;
            targetPos = imitatorGameObject.transform.position;
        }

        public ImitatorState ImitatorState
        {
            get { return imitatorState; }
            set { imitatorState = value; }
        }

        public GameObject ImitatorGameObject
        {
            get { return imitatorGameObject; }
            set { imitatorGameObject = value; }
        }

        public void ImitatePlayerMove(PlayerController player)
        { // TODO: debug 一下
            if (!isMoving)
            {
                if (player.playerState == PlayerState.Moving)
                { // 玩家正在移动
                    Vector3 pos = Vector3.Normalize(player.targetPosition - player.transform.position) +
                                  imitatorGameObject.transform.position;
                    if (Ground.Accessible(imitatorGameObject.transform.position) && Ground.Accessible(pos))
                    { // 当前位置以及目标位置可通行
                        targetPos = pos;
                        isMoving = true;
                        imitatorGameObject.transform.position = Vector3.MoveTowards(
                            imitatorGameObject.transform.position, targetPos, Time.deltaTime * movingSpeed);
                        imitatorState = ImitatorState.Moving;
                    }
                }
            }
            else
            { // 移动中
                if (!imitatorGameObject.transform.position.Equals(targetPos))
                { // 还没有达到目的地
                    imitatorGameObject.transform.position = Vector3.MoveTowards(
                        imitatorGameObject.transform.position, targetPos, Time.deltaTime * movingSpeed);
                }
                else
                { // 到达目的地
                    isMoving = false;
                    imitatorState = ImitatorState.Idle;
                }
            }
        }
        
        public IEnumerator BlinkAndDestroy(float blinkDuration, float cycle)
        {
            bool state = true;
            float elapse = 0;
            imitatorState = ImitatorState.Dead;
            ImitatorGameObject.GetComponent<Collider>().enabled = false;
            while (elapse < blinkDuration)
            {
                ImitatorGameObject.GetComponent<Renderer>().enabled = !state;
                state = !state;
                elapse += cycle / 2;
                yield return new WaitForSeconds(cycle/2);
            }
            ImitatorGameObject.GetComponent<Renderer>().enabled = !state;
            ImitatorGameObject.SetActive(false);
            yield return null;
        }
    }
}
