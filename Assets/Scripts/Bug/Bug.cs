using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Bug
{
    public class Bug : MonoBehaviour
    {
        public StinkManager manager;
        public float speed;
        
        public float rotationChangeTime;
        private float rotationChangeTimerHold;

        public Transform bugTransform;

        public float spreadAmount, spreadTimeInterval = 1f;
        public int radius = 5;

        public TileBase fromTileBase;
        public TileBase toTileBase;
        public Vector3Int lastPos;
        
        private void Start()
        {
            bugTransform = transform;
        }

        private void Update()
        {
            rotationChangeTimerHold -= Time.deltaTime;

            if (rotationChangeTimerHold <= 0)
            {
                ChangeRotation();
            }
            

            Move();
        }

        private void Move()
        {
            var position = bugTransform.position;
            var disposition = position;
            disposition += bugTransform.up * (Time.deltaTime * speed);

            if (CheckForBorder(ref disposition) == false)
                return;

            position = disposition;
            bugTransform.position = position;

            var gridPosition = manager.gameManager.map.WorldToCell(position);
            if (lastPos != gridPosition)
            {
                lastPos = gridPosition;
                AddStink(transform.position, spreadAmount);
                RemoveStink();
            }


        }

        private bool CheckForBorder(ref Vector3 pos)
        {
            if (pos.x < manager.gameManager.maxBorderX && pos.x > manager.gameManager.minBorderX &&
                pos.y < manager.gameManager.maxBorderY && pos.y > manager.gameManager.minBorderY)
                return true;
            
            
            transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z - 180);
            return false;
        }

        

        private void ChangeRotation()
        {
            rotationChangeTimerHold = rotationChangeTime;
            var newRotation = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
        }

        private void AddStink(Vector3 transformPosition, float stinkAmount)
        {
            Vector3Int gridPosition = manager.map.WorldToCell(transformPosition);

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);
                    if(distanceFromCenter <= radius)
                    {
                        var nextTilePosition = new Vector3Int(gridPosition.x + x, gridPosition.y + y, 0);
                        
                        ChangeFireAsh(ref nextTilePosition);
                        ChangeStink(nextTilePosition, stinkAmount - (distanceFromCenter * manager.stinkFallOff * stinkAmount));
                    }
                }
            }
            VisualizeStink();

        }

        private void ChangeFireAsh(ref Vector3Int nextTilePosition)
        {
            var worldCellPos = manager.map.GetCellCenterWorld(nextTilePosition);
            
            var centerPosNextTile = manager.gameManager.map.WorldToCell(worldCellPos);
            
            var gridBase = manager.gameManager.map.GetTile(centerPosNextTile);
            if (gridBase == null)
                return;

            if (gridBase == fromTileBase)
            {
                manager.gameManager.map.SetTile(centerPosNextTile, toTileBase);
            }
        }

        private void RemoveStink()
        {
            var stinkingTilesCopy = new Dictionary<Vector3Int, float>(manager.stinkingTiles);
            
            foreach (var entry in stinkingTilesCopy)
            {   
                ChangeStink(entry.Key, manager.reduceAmount);
            }

            VisualizeStink();

        }

        private void VisualizeStink()
        {
            foreach (var entry in manager.stinkingTiles)
            {
                float stinkPercent = entry.Value / manager.maxStink;

                Color newTileColor = manager.maxStinkColor * stinkPercent + manager.minStinkColor * (1f - stinkPercent);

                manager.map.SetTileFlags(entry.Key, TileFlags.None);
                manager.map.SetColor(entry.Key, newTileColor);
                manager.map.SetTileFlags(entry.Key, TileFlags.LockColor);
            }
        }


        private void ChangeStink(Vector3Int nextTilePosition, float managerStinkFallOff)
        {
            
            manager.stinkingTiles.TryAdd(nextTilePosition, 0f);

            float newValue = manager.stinkingTiles[nextTilePosition] + managerStinkFallOff;

            if (newValue <= 0f)
            {
                manager.stinkingTiles.Remove(nextTilePosition);

                manager.map.SetTileFlags(nextTilePosition, TileFlags.None);
                manager.map.SetColor(nextTilePosition, manager.clearColor);
                manager.map.SetTileFlags(nextTilePosition, TileFlags.LockColor);
            
            }
            else
                manager.stinkingTiles[nextTilePosition] = Mathf.Clamp(newValue, 0f, manager.maxStink);
        }
    }
}