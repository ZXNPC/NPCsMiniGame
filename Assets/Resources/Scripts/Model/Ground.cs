using System;
using System.Collections.Generic;
using Resources.Scripts.Enum;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Resources.Scripts.Model
{
    public static class Ground
    {
        private static GameObject groundGameObject;
        // ↓ key.type = Vector3.class, value.type = Tile.class
        private static Hashtable tiles;
        // TODO: 或许优化的时候这里可以用空间换时间，再声明喷涂地面，未喷涂地面和普通地面
        private static List<Vector3> lockedTileList;

        private static float minX;
        private static float maxX;
        private static float minZ;
        private static float maxZ;
        private static float interval;
        private static float width;
        private static float height;

        private static Material tileNormalMaterial;
        private static Material tileImpassibleMaterial;
        private static Material tilePaintedMaterial;
        private static Material tileUnpaintedMaterial;

        public static void Initialize(GameObject ground)
        {
            groundGameObject = ground;
            Transform[] tileTransforms = groundGameObject.GetComponentsInChildren<Transform>().Skip(1).ToArray();
            tileNormalMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile_Normal");
            tileImpassibleMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile_Impassible");
            tilePaintedMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile_Painted");
            tileUnpaintedMaterial = UnityEngine.Resources.Load<Material>("Materials/Tile_Unpainted");

            tiles = new Hashtable();
            foreach (Transform tileTransform in tileTransforms)
            { // TODO: 暂时使用材质名称来进行比较
                TileType tileType = TileType.Normal;
                Material material = tileTransform.GetComponent<Renderer>().material;
                if (material.name.Equals(tileNormalMaterial.name + " (Instance)"))
                {
                    tileType = TileType.Normal;
                }
                else if (material.name.Equals(tileImpassibleMaterial.name + " (Instance)"))
                {
                    tileType = TileType.Impassible;
                }
                else if (material.name.Equals(tilePaintedMaterial.name + " (Instance)"))
                {
                    tileType = TileType.Painted;
                }
                else if (material.name.Equals(tileUnpaintedMaterial.name + " (Instance)"))
                {
                    tileType = TileType.Unpainted;
                }
                else { }
                Vector3 position = tileTransform.position;
                tiles.Add(new Vector3(position.x, 0, position.z),
                    new Tile(tileType, tileTransform.gameObject));
            }

            // TODO: 目前就使用 tag 来标明被锁上的地块
            lockedTileList = new List<Vector3>();
            foreach (GameObject lockedTile in GameObject.FindGameObjectsWithTag("LockedTile"))
            {
                Vector3 pos = lockedTile.transform.position;
                lockedTileList.Add(new Vector3(pos.x, 0, pos.z));
            }

            minX = Int32.MaxValue;
            maxX = Int32.MinValue;
            minZ = Int32.MaxValue;
            maxZ = Int32.MinValue;
            foreach (Vector3 tilePos in tiles.Keys)
            {
                minX = minX < tilePos.x ? minX : tilePos.x;
                maxX = maxX > tilePos.x ? maxX : tilePos.x;
                minZ = minZ < tilePos.x ? minZ : tilePos.x;
                maxZ = maxZ > tilePos.x ? maxZ : tilePos.x;
            }

            interval = 1; // 间隔默认为1
            width = maxX - minX + interval;
            height = maxZ - minZ + interval;
        }

        static public float MinX { get { return minX; } }

        static public float MaxX { get { return maxX; } }

        static public float MinZ { get { return minZ; } }

        static public float MaxZ { get { return maxZ; } }

        static public float Interval { get { return interval; } }

        static public float Width { get { return width; } }

        static public float Height { get { return height; } }




        private static bool IsOutOfGround(Vector3 pos)
        {
            // 地面边界判断，不一定用得上，总之我先写一个
            if (pos.x >= minX && pos.x <= maxX && pos.z >= minZ && pos.z <= maxZ)
            {
                return false;
            }

            return true;
        }

        static public bool Accessible(Vector3 pos)
        {
            // 检测当前位置是否在不可通行地面上
            if (IsOutOfGround(pos))
            {
                return false;
            }

            if (((Tile)tiles[new Vector3(pos.x, 0, pos.z)]).TileType == TileType.Impassible)
            {
                return false;
            }

            return true;
        }

        private static bool IsAllPainted()
        {
            foreach (Tile tile in tiles.Values)
            {
                if (tile.TileType == TileType.Unpainted)
                {
                    return false;
                }
            }
            return true;
        }

        static public void UnlockTile(Vector3 pos)
        {
            if (lockedTileList.Contains(pos))
            {
                Tile tile = (Tile)tiles[pos];
                tile.TileGameObject.GetComponent<Renderer>().material = tileNormalMaterial;
                tile.TileType = TileType.Normal;
            }
            else
            {
                throw new Exception("该位置的地板的 tag 不为 'LockedTile'");
            }
        }

        static public void Paint(Vector3 pos)
        {
            if (tiles.ContainsKey(new Vector3(pos.x, 0, pos.z)))
            { // 此处可以涂地
                Tile tile = (Tile)tiles[new Vector3(pos.x, 0, pos.z)];
                if (tile.TileType == TileType.Unpainted)
                { // 涂地
                    tile.TileType = TileType.Painted;
                    tile.TileGameObject.GetComponent<Renderer>().material = tilePaintedMaterial;
                    if (IsAllPainted())
                    { // 全部地面完成涂地
                        foreach (Vector3 lockedTilePos in lockedTileList)
                        {
                            ((Tile)tiles[lockedTilePos]).TileGameObject.GetComponent<Renderer>().material =
                                tileNormalMaterial;
                            ((Tile)tiles[lockedTilePos]).TileType = TileType.Normal;
                        }
                        lockedTileList.Clear();
                    }
                }
            }
            else
            {
                Debug.Log("未在地板位置上涂地");
            }
        }

        static public IEnumerator Change(float cycle)
        {
            bool state = true;
            while (true)
            {
                foreach (Tile tile in tiles.Values)
                {
                    if (state)
                    {
                        tile.TileGameObject.GetComponent<Renderer>().material = tileImpassibleMaterial;
                        tile.TileType = TileType.Impassible;
                    }
                    else
                    {
                        tile.TileGameObject.GetComponent<Renderer>().material = tileNormalMaterial;
                        tile.TileType = TileType.Normal;
                    }
                }
                state = !state;
                Debug.Log("change");
                yield return new WaitForSeconds(cycle / 2);
            }
        }
    }
}
