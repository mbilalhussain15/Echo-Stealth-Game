using System.Collections.Generic;
using UnityEngine;

public class RechargeStation : MonoBehaviour
{
    public int rechargeAmount = 20;
    private GridManager gridManager;
    public static List<int> usedColumns = new List<int>();
    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            return;
        }

        PlaceInConnectedRows();
    }
    void PlaceInConnectedRows()
    {
        List<Vector2Int> validSlots = new List<Vector2Int>();

       
        for (int row = 1; row <= 3; row++)
        {
            for (int col = 0; col < gridManager.cols; col++)
            {
                Tile tile = gridManager.grid[row, col];
                if (tile != null && tile.isConnected && !usedColumns.Contains(col))
                {
                    validSlots.Add(new Vector2Int(row, col));
                }
            }
        }

        if (validSlots.Count > 0)
        {
            
            Vector2Int selectedSlot = validSlots[Random.Range(0, validSlots.Count)];
            usedColumns.Add(selectedSlot.y);

           
            Tile targetTile = gridManager.grid[selectedSlot.x, selectedSlot.y];
            transform.position = targetTile.transform.position;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<EnergySystem>().RechargeEnergy(rechargeAmount);
        }
    }
}