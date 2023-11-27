using Resources.Scripts.Enum;
using UnityEngine;

namespace Resources.Scripts.Model
{
    public class Tile
    {
        private TileType tileType;
        private GameObject tileGameObject;

        public Tile(TileType tileType, GameObject tileGameObject)
        {
            this.tileType = tileType;
            this.tileGameObject = tileGameObject;
        }

        public TileType TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }

        public GameObject TileGameObject
        {
            get { return tileGameObject; }
            set { tileGameObject = value; }
        }
    }
}