using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitNode : MonoBehaviour
{
    private GridManager gridManager;
    public int exitNodeColumn;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            return;
        }

        SetExitNodePosition(0);
    }
    void SetExitNodePosition(int targetRow)
    {
        List<int> validColumns = new List<int>();

        for (int col = 0; col < gridManager.cols; col++)
        {
            Tile tile = gridManager.grid[targetRow, col];
            if (tile != null && tile.isConnected)
            {
                validColumns.Add(col);
            }
        }

        if (validColumns.Count > 0)
        {
            int selectedCol = validColumns[Random.Range(0, validColumns.Count)];
            Tile selectedTile = gridManager.grid[targetRow, selectedCol];
            transform.position = selectedTile.transform.position;
            exitNodeColumn = selectedCol; 
            
        }
        else
        {
          
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            FindFirstObjectByType<EndingManager>().TriggerEnding();
        }
    }
}