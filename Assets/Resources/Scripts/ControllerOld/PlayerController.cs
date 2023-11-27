// using System.Collections;
// using System.Collections.Generic;
// using Resources.Scripts.Enum;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class Player : MonoBehaviour
// {
//     public GameObject goal;
//     public MeshRenderer meshRenderer;
//     public float movingTime;
//     public float movingPace;
//
//     public PlayerState playerState;
//     private List<Vector2> groundPosList = new List<Vector2>();
//     private float movingElapse = 0;
//     private Vector3 curPos;
//
//     
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         goal = GameObject.Find("Goal");
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
//         playerState = PlayerState.Idle;
//         curPos = transform.position;
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         Move();
//         IsReach();
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
//     private void Move()
//     {
//         if (playerState != PlayerState.Dead)
//         {
//             if ((Input.GetKeyDown(KeyCode.W) && playerState == PlayerState.Idle || playerState == PlayerState.MovingForward) && transform.position.z != 1 &&
//                 !groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z + 1))))
//             {   // 向前移动
//                 if (movingElapse < movingTime)
//                 {   // 移动中
//                     if (!groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z))))
//                     {   // 当前位置不存在 ground
//                         playerState = PlayerState.MovingForward;
//                         transform.Translate(Vector3.forward * movingPace * (1 / movingTime) * Time.deltaTime);
//                         movingElapse += Time.deltaTime;
//                     }
//                 }
//                 else
//                 {   // 移动结束
//                     curPos = new Vector3(curPos.x, curPos.y, curPos.z + 1);
//                     transform.position = curPos;
//                     playerState = PlayerState.Idle;
//                     movingElapse = 0;
//                 }
//             }
//
//             else if ((Input.GetKeyDown(KeyCode.S) && playerState == PlayerState.Idle || playerState == PlayerState.MovingBack) && transform.position.z != -1 &&
//                 !groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z - 1))))
//             {
//                 if (movingElapse < movingTime)
//                 {
//                     if (!groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z))))
//                     {
//                         playerState = PlayerState.MovingBack;
//                         transform.Translate(Vector3.back * movingPace * (1 / movingTime) * Time.deltaTime);
//                         movingElapse += Time.deltaTime;
//                     }
//                 }
//                 else
//                 {
//                     curPos = new Vector3(curPos.x, curPos.y, curPos.z - 1);
//                     transform.position = curPos;
//                     playerState = PlayerState.Idle;
//                     movingElapse = 0;
//                 }
//             }
//
//             else if ((Input.GetKeyDown(KeyCode.A) && playerState == PlayerState.Idle || playerState == PlayerState.MovingLeft) && transform.position.x != -1 &&
//                 !groundPosList.Exists(v => v.Equals(new Vector2(curPos.x - 1, curPos.z))))
//             {
//                 if (movingElapse < movingTime)
//                 {
//                     if (!groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z))))
//                     {
//                         playerState = PlayerState.MovingLeft;
//                         transform.Translate(Vector3.left * movingPace * (1 / movingTime) * Time.deltaTime);
//                         movingElapse += Time.deltaTime;
//                     }
//                 }
//                 else
//                 {
//                     curPos = new Vector3(curPos.x - 1, curPos.y, curPos.z);
//                     transform.position = curPos;
//                     playerState = PlayerState.Idle;
//                     movingElapse = 0;
//                 }
//             }
//
//             else if ((Input.GetKeyDown(KeyCode.D) && playerState == PlayerState.Idle || playerState == PlayerState.MovingRight) && transform.position.x != 1 &&
//                 !groundPosList.Exists(v => v.Equals(new Vector2(curPos.x + 1, curPos.z))))
//             {
//                 if (movingElapse < movingTime)
//                 {
//                     if (!groundPosList.Exists(v => v.Equals(new Vector2(curPos.x, curPos.z))))
//                     {
//                         playerState = PlayerState.MovingRight;
//                         transform.Translate(Vector3.right * movingPace * (1 / movingTime) * Time.deltaTime);
//                         movingElapse += Time.deltaTime;
//                     }
//                 }
//                 else
//                 {
//                     curPos = new Vector3(curPos.x + 1, curPos.y, curPos.z);
//                     transform.position = curPos;
//                     playerState = PlayerState.Idle;
//                     movingElapse = 0;
//                 }
//             }
//         }
//     }
//
//     private void MoveMove()
//     {
//         
//     }
//
//     private void IsReach()
//     {
//         if (transform.position == goal.transform.position)
//         {
//             SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
//         }
//     }
//
//     public void Killed()
//     {
//         playerState = PlayerState.Dead;
//         StartCoroutine(BlinkAndRestart());
//     }
//
//     IEnumerator BlinkAndRestart(float duration = 1)
//     {
//         float cycle = 0.5f;
//         float elapse = 0;
//         Color originalColor = meshRenderer.material.color;
//         while (elapse < duration)
//         {
//             print(elapse);
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
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }
// }