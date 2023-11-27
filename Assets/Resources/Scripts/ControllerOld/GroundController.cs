// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEditor.VersionControl;
// using UnityEngine;
// using UnityEngine.Assertions;
//
// public class Ground : MonoBehaviour
// {
//     public Material unpaintedMaterial;
//     public Material paintedMaterial;
//     public Material groundMaterial;
//     public Material tileMaterial;
//     public GameObject player;
//     public GameObject goal;
//
//     private List<Transform> unpaintedList = new List<Transform>();
//     private static List<Vector3> unpaintedPosList = new List<Vector3>();
//     private List<Transform> paintedList = new List<Transform>();
//     private static List<Vector3> paintedPosList = new List<Vector3>();
//     private List<Transform> groundList = new List<Transform>();
//     private static List<Vector3> groundPosList = new List<Vector3>();
//
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         unpaintedMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile UnPainted Material");
//         paintedMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile Painted Material");
//         groundMaterial = UnityEngine.Resources.Load<Material>("Materials/Ground Material");
//         tileMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile Material");
//         player = GameObject.Find("Player");
//         goal = GameObject.Find("Goal");
//         InitList();
//     }
//
//     void InitList()
//     {
//         for (int i = 0; i < gameObject.transform.childCount; i++)
//         {
//             // 这里我直接用材质的名称来做判断了
//             Transform tile = gameObject.transform.GetChild(i);
//             if (tile.GetComponent<MeshRenderer>().material.name.Equals(unpaintedMaterial.name + " (Instance)"))
//             {
//                 unpaintedList.Add(tile);
//                 unpaintedPosList.Add(tile.position);
//             }
//             else if (tile.GetComponent<MeshRenderer>().material.name.Equals(paintedMaterial.name + " (Instance)"))
//             {
//                 paintedList.Add(tile);
//                 paintedPosList.Add(tile.position);
//             }
//             else if (tile.GetComponent<MeshRenderer>().material.name.Equals(groundMaterial.name + " (Instance)"))
//             {
//                 groundList.Add(tile);
//                 groundPosList.Add(tile.position);
//             }
//             else
//             {
//             }
//         }
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         Paint();
//         Unlock();
//     }
//
//     void Paint()
//     {
//         if (unpaintedList.Count != 0)
//         {
//             Vector2 playerPos = player.transform.position;
//             if (unpaintedPosList.Contains(playerPos))
//             {
//                 // 涂地
//                 int i = unpaintedPosList.IndexOf(playerPos);
//                 Assert.IsTrue(i >= 0);
//                 unpaintedList[i].GetComponent<Renderer>().material = paintedMaterial;
//                 paintedList.Add(unpaintedList[i]);
//                 paintedPosList.Add(unpaintedPosList[i]);
//                 unpaintedList.RemoveAt(i);
//                 unpaintedPosList.RemoveAt(i);
//             }
//         }
//     }
//
//     void Unlock()
//     {
//         // 解锁指定地盘
//         if (unpaintedList.Count == 0 && paintedList.Count != 0 && groundPosList.Exists(v =>
//                 v.Equals(new Vector2(goal.transform.position.x, goal.transform.position.z))))
//         {
//             int i = groundPosList.IndexOf(new Vector2(goal.transform.position.x, goal.transform.position.z));
//             groundList[i].GetComponent<Renderer>().material = tileMaterial;
//             groundList.RemoveAt(i);
//             groundPosList.RemoveAt(i);
//             GameObject.Find("Player").GetComponent<Player>().InitGroundPos(groundList);
//             GameObject.Find("Imitator").GetComponent<Imitator>().InitGroundPos(groundList);
//             GameObject.Find("Blocker").GetComponent<Blocker>().InitGroundPos(groundList);
//         }
//     }
//
//     public bool GroundPosListContarin(Vector3 pos)
//     {
//         return groundPosList.Contains(pos);
//     }
// }