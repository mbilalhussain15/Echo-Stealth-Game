using System.Collections.Generic;
using UnityEngine;

public class SecurityNode : MonoBehaviour
{
    private bool isHacked = false;
    private SpriteRenderer spriteRenderer;
    private static bool isAlreadyPlaced = false;
   
    void Start()
    {

        if (!isAlreadyPlaced)
        {
            PlaceInSecondRow();
            isAlreadyPlaced = true; 
        }
        else
        {
            Destroy(gameObject); 
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
   
    public static void ResetPlacement()
    {
        isAlreadyPlaced = false;
    }
    void PlaceInSecondRow()
    {
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null) return;

        List<Vector2Int> validSlots = new List<Vector2Int>();

        for (int col = 0; col < gridManager.cols; col++)
        {
            Tile tile = gridManager.grid[1, col];
            if (tile != null && tile.isConnected && !RechargeStation.usedColumns.Contains(col))
            {
                validSlots.Add(new Vector2Int(1, col));
            }
        }

        if (validSlots.Count > 0)
        {
           
            Vector2Int selectedSlot = validSlots[Random.Range(0, validSlots.Count)];
            RechargeStation.usedColumns.Add(selectedSlot.y); 
            transform.position = gridManager.grid[selectedSlot.x, selectedSlot.y].transform.position;
           
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
         
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isHacked && Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 1.5f)
        {
            isHacked = true;
            spriteRenderer.color = Color.red; 
            
        }
    }
}
