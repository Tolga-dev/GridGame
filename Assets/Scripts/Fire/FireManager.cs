using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Fire;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FireManagerGrid : MonoBehaviour
{
    

    public TileBase fireAshBase;

    public List<Vector3Int> activeFires = new List<Vector3Int>();

    public GameObject firePrefab;

    public GameManager manager;
    
    public void SetTileOnFire(Vector3Int gridPosition, ref TileDataGrid dataGrid)
    {
        var centerPos = manager.map.GetCellCenterWorld(gridPosition);
        centerPos.z = 0;

        var newFireObject = Instantiate(firePrefab);
        var newFire = newFireObject.GetComponentInChildren<FireInGame>();

        newFire.transform.position = centerPos;
        gridPosition.z = 0;
        newFire.StartBurning(ref gridPosition, this, ref dataGrid);
        
        activeFires.Add(gridPosition);
    }
    

    public void FinishedBurning(Vector3Int position)
    {
        
        manager.map.SetTile(position, fireAshBase);
        activeFires.Remove(position);
    }

    public void TryToSpread(Vector3Int position)
    {
        for (int x = position.x -1; x < position.x + 2 ; x++)
        {
            for (int y = position.y - 1; y < position.y + 2; y++)
            {
                TryToBurnTile(new Vector3Int(x, y, 0));
            }
        }
    }
    
    void TryToBurnTile(Vector3Int tilePosition)
    {
        if (activeFires.Contains(tilePosition)) return;

        var data = manager.GetTileData(tilePosition);
        
        if (data != null && data.burnable.canBurn)
        {
            if (UnityEngine.Random.Range(0f, 100f) <= data.burnable.spreadChance*100)
                SetTileOnFire(tilePosition, ref data);    
        }

    }

}
