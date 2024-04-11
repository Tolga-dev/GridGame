using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class SelectMaterial : MonoBehaviour
{
    public List<Material> materials = new List<Material>();
    public GameManager manager;

    public TileDataGrid currentMaterial;
    public int currentMaterialId;
    public GameObject currentPrefab;
    
    private void Start()
    {
        foreach (var material in materials)
        {
            material.selector = this;
            material.materialData = manager.tiles[material.id];
        }
    }
    
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
            {
            }
            else
            {
                SpawnObject();
            }
        } 
        
    }
 

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    private void SpawnObject()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var gridPosition = manager.map.WorldToCell(mousePosition);
        gridPosition.z = 0;
        var data = manager.GetTileData(gridPosition);

        switch (currentMaterialId)
        {
            case 0:
                manager.fireManagerGrid.SetTileOnFire(gridPosition,ref data);
                break;
            case 4:
                SpawnBugObject(ref gridPosition);
                break;
            default:
                manager.map.SetTile(gridPosition,currentMaterial.tiles[0]);
                break;
        }
        
    }

    private void SpawnBugObject(ref Vector3Int gridPos)
    {
        currentMaterial = null;
        var bug = Instantiate(currentPrefab, gridPos,Quaternion.identity);
        var myBug = bug.GetComponent<Bug.Bug>();
        myBug.manager = manager.stinkManager;

    }
    
}
