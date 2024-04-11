using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    [Serializable]
    [CreateAssetMenu(fileName = "TileDataGrid", menuName = "TileDataGrid", order = 0)]
    public class TileDataGrid : ScriptableObject
    {
        public TileBase[] tiles;
        [SerializeField]public Burnable burnable = new Burnable();
    }

    [Serializable]
    public class Burnable
    {
        public bool canBurn;
        public float spreadChance, spreadInterval, burnTime;
    }
}