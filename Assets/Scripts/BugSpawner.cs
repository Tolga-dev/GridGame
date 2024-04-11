using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BugSpawner : MonoBehaviour,IPointerClickHandler
{
    public GameObject prefab;
    public int id;
    public SelectMaterial selector;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        selector.currentMaterialId = id;
        selector.currentPrefab = prefab;
    }
    
    
}
