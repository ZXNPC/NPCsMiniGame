// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Blocker : MonoBehaviour
// {
//     public GameObject player;
//     public GameObject imitator;
//     public int[] pertrolRoute;
//     public float waitingTime = 1;
//     public MeshRenderer meshRenderer;
//
//     private int curPertrolRoute;
//     private float curTime;
//     private BlockerState blockerState;
//     private List<Vector2> groundPosList = new List<Vector2>();
//
//     private enum BlockerState
//     {
//         Moving,
//         Dead
//     }
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         player = GameObject.Find("Player");
//         imitator = GameObject.Find("Imitator");
//         meshRenderer = gameObject.GetComponent<MeshRenderer>();
//         blockerState = BlockerState.Moving;
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
//     void Move()
//     {
//         Vector3 curPos = transform.position;
//         if (blockerState == BlockerState.Moving && !groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z))))
//         {
//             curTime += Time.deltaTime;
//             if (curTime >= waitingTime)
//             {
//                 transform.position = RouteToPos(pertrolRoute[curPertrolRoute]);
//                 if (curPertrolRoute < pertrolRoute.Length - 1)
//                 {
//                     curPertrolRoute++;
//                 }
//                 else
//                 {
//                     curPertrolRoute = 0;
//                 }
//
//                 curTime = 0;
//             }
//         }
//     }
//
//     Vector3 RouteToPos(int route)
//     {
//         int x, z;
//         switch (route)
//         {
//             case 1:
//             {
//                 x = -1;
//                 z = 1;
//                 break;
//             }
//             case 2:
//             {
//                 x = 0;
//                 z = 1;
//                 break;
//             }
//             case 3:
//             {
//                 x = 1;
//                 z = 1;
//                 break;
//             }
//             case 4:
//             {
//                 x = -1;
//                 z = 0;
//                 break;
//             }
//             case 5:
//             {
//                 x = 0;
//                 z = 0;
//                 break;
//             }
//             case 6:
//             {
//                 x = 1;
//                 z = 0;
//                 break;
//             }
//             case 7:
//             {
//                 x = -1;
//                 z = -1;
//                 break;
//             }
//             case 8:
//             {
//                 x = 0;
//                 z = -1;
//                 break;
//             }
//             case 9:
//             {
//                 x = 1;
//                 z = -1;
//                 break;
//             }
//             default:
//             {
//                 x = 0;
//                 z = 0;
//                 Debug.Log("出错力");
//                 break;
//             }
//         }
//
//         return new Vector3(x, transform.position.y, z);
//     }
//
//     void IsBlock()
//     {
//         if (player.transform.position == transform.position && blockerState != BlockerState.Dead)
//         {
//             GameObject.Find("Player").GetComponent<Player>().Killed();
//             blockerState = BlockerState.Dead;
//             StartCoroutine(BlinkAndDestroy());
//         }
//         else if (imitator.transform.position == transform.position && blockerState != BlockerState.Dead)
//         {
//             GameObject.Find("Imitator").GetComponent<Imitator>().Killed();
//             blockerState = BlockerState.Dead;
//             StartCoroutine(BlinkAndDestroy());
//         }
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