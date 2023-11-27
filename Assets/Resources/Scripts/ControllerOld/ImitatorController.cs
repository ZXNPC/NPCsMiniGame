// using System;
// using System.Collections;
// using System.Collections.Generic;
// using OpenCover.Framework.Model;
// using Resources.Scripts;
// using Resources.Scripts.Enum;
// using UnityEngine;
//
// public class Imitator : MonoBehaviour
// {
//     public MeshRenderer meshRenderer;
//     public float movingTime;
//     public float movingPace;
//     public GameObject player;
//     public Boundary boundary;
//
//     private ImitatorState imitatorState;
//     private List<Vector2> groundPosList = new List<Vector2>();
//     private float movingElapse = 0;
//     private Vector3 curPos;
//     private Vector2 targetPos;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         meshRenderer = gameObject.GetComponent<MeshRenderer>();
//         if (movingTime == 0)
//         {
//             movingTime = 0.5f;
//         }
//
//         if (movingPace == 0)
//         {
//             movingPace = 1;
//         }
//
//         player = GameObject.Find("Player");
//         boundary = new Boundary(-1, 1, -1, 1);
//         imitatorState = ImitatorState.Idle;
//         curPos = transform.position;
//         targetPos = new Vector2(curPos.x, curPos.z);
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         Move();
//         IsBlock();
//     }
//
//     public void InitGroundPos(List<Transform> groundList)
//     {
//         groundPosList.Clear();
//         for (int i = 0; i < groundList.Count; i++)
//         {
//             Vector3 tilePos = groundList[i].position;
//             groundPosList.Add(new Vector2(tilePos.x, tilePos.z));
//         }
//     }
//
//     ImitatorState imitatePlayer(PlayerState playerState)
//     {
//         switch (playerState)
//         {
//             case PlayerState.Idle: return ImitatorState.Idle;
//             case PlayerState.MovingForward: return ImitatorState.MovingForward;
//             case PlayerState.MovingBack: return ImitatorState.MovingBack;
//             case PlayerState.MovingLeft: return ImitatorState.MovingLeft;
//             case PlayerState.MovingRight: return ImitatorState.MovingRight;
//             default: return ImitatorState.Idle;
//         }
//     }
//
//     private void Move()
//     {
//         imitatorState = imitatePlayer(GameObject.Find("Player").GetComponent<Player>().playerState);
//         curPos = transform.position;
//         if (targetPos.Equals(new Vector2(curPos.x, curPos.z)))
//         {
//             switch (imitatorState)
//             {
//                 case ImitatorState.MovingForward:
//                 {
//                     print("向前");
//                     targetPos = new Vector2(curPos.x, curPos.z + 1);
//                     break;
//                 }
//                 case ImitatorState.MovingBack:
//                 {
//                     print("向后");
//                     targetPos = new Vector2(curPos.x, curPos.z - 1);
//                     break;
//                 }
//                 case ImitatorState.MovingLeft:
//                 {
//                     print("向左");
//                     targetPos = new Vector2(curPos.x - 1, curPos.z);
//                     break;
//                 }
//                 case ImitatorState.MovingRight:
//                 {
//                     print("向右");
//                     targetPos = new Vector2(curPos.x + 1, curPos.z);
//                     break;
//                 }
//                 default:
//                 {
//                     targetPos = new Vector2(curPos.x, curPos.z);
//                     break;
//                 }
//             }
//
//             if (groundPosList.Contains(targetPos) || boundary.OutOfBoundry(targetPos))
//             {
//                 targetPos = new Vector2(curPos.x, curPos.z);
//                 imitatorState = ImitatorState.Idle;
//             }
//         }
//         else
//         {
//             transform.position = Vector3.MoveTowards(transform.position,
//                 new Vector3(targetPos.x, curPos.y, targetPos.y), Time.deltaTime * movingPace * (1 / movingTime));
//             
//         }
//     }
//
//     void IsBlock()
//     {
//         if (player.transform.position == transform.position && imitatorState != ImitatorState.Dead)
//         {
//             GameObject.Find("Player").GetComponent<Player>().Killed();
//             imitatorState = ImitatorState.Dead;
//             StartCoroutine(BlinkAndDestroy());
//         }
//     }
//
//     public void Killed()
//     {
//         imitatorState = ImitatorState.Dead;
//         StartCoroutine(BlinkAndDestroy());
//     }
//
//     IEnumerator BlinkAndDestroy(float duration = 1)
//     {
//         float cycle = 0.5f;
//         float elapse = 0;
//         Color originalColor = meshRenderer.material.color;
//         while (elapse < duration)
//         {
//             if (elapse % cycle < cycle / 2)
//             {
//                 meshRenderer.material.color = Color.white;
//             }
//             else
//             {
//                 meshRenderer.material.color = originalColor;
//             }
//
//             elapse += Time.deltaTime;
//
//             yield return null;
//         }
//
//         meshRenderer.material.color = originalColor;
//         DestroyImmediate(gameObject);
//         yield return null;
//     }
// }