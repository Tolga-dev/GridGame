using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class Material : MonoBehaviour, IPointerClickHandler
{
    public int id;
    public SelectMaterial selector;
    public TileDataGrid materialData; 
    
    public void OnPointerClick(PointerEventData eventData)
    {
        selector.currentMaterial = materialData;
        selector.currentMaterialId = id;
    }
}
