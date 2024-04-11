using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public FireManagerGrid fireManagerGrid;
    public StinkManager stinkManager;
    public List<TileDataGrid> tiles;
    public Dictionary<TileBase, TileDataGrid> dataGrids;
    public Tilemap map;

    public float maxBorderY;
    public float minBorderY;
        
    public float minBorderX;
    public float maxBorderX;

    private void Awake()
    {
        
        dataGrids = new Dictionary<TileBase, TileDataGrid>();

        foreach (var tileData in tiles)
        {
            foreach (var tile in tileData.tiles)
            {
                dataGrids.Add(tile, tileData);
            }
        }
        
    }
    
    
    public TileDataGrid GetTileData(Vector3Int tilePosition)
    {
        var tile = map.GetTile(tilePosition);
        
        if (tile == null)
        {
            return null;
        }
        else
            return dataGrids[tile];
        
    }

}
