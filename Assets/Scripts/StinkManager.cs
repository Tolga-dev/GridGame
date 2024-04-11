using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class StinkManager : MonoBehaviour
    {
        public GameManager gameManager;
        public Tilemap map;
        public float stinkFallOff = 0.1f;
        public readonly Dictionary<Vector3Int, float> stinkingTiles = new Dictionary<Vector3Int, float>();

        public Color maxStinkColor, minStinkColor, clearColor;

        public float maxStink;
        public float reduceAmount;
    }
}