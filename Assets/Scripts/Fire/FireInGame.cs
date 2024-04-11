using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace.Fire
{
    public class FireInGame : MonoBehaviour
    {
        
        private Vector3Int position;

        private FireManagerGrid fireManager;

        private float burnTimeCounter, spreadInterval;
        private TileDataGrid dataGrid;

        public void StartBurning(ref Vector3Int position, FireManagerGrid fm, ref TileDataGrid tileDataGrid)
        {
            this.position = position;
            dataGrid = tileDataGrid;
            fireManager = fm;

            burnTimeCounter = tileDataGrid.burnable.burnTime;
            spreadInterval = tileDataGrid.burnable.spreadInterval;
        }

        private void Update()
        {
            burnTimeCounter -= Time.deltaTime;
            if(burnTimeCounter <=0)
            {
                fireManager.FinishedBurning(position);
                Destroy(gameObject);
            }
            
            spreadInterval -= Time.deltaTime;
            if(spreadInterval <=0)
            {
                spreadInterval = dataGrid.burnable.spreadInterval;
                fireManager.TryToSpread(position);
            }
            
        
        }

    }
}