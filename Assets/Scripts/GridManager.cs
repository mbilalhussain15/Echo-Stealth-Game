using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int rows = 8;
    [SerializeField] public int cols = 15;
    [SerializeField] public float tileSize = 1.2f;
    [SerializeField] private GameObject horizontalPipePrefab;
    [SerializeField] private GameObject verticalPipePrefab;

    public Tile[,] grid;

    public bool IsGridInitialized { get; private set; } = false;


   
    void Start()
    {
        GenerateGrid(); 
        IsGridInitialized = true; 
        
    }
    void GenerateGrid()
    {
        grid = new Tile[rows, cols];
        GameObject referenceTile = Instantiate(Resources.Load("gridImage")) as GameObject;
        Vector2 gridOffset = new Vector2(
            (cols - 1) * tileSize / 2,
            (rows - 1) * -tileSize / 2
        );

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector2 tilePos = new Vector2(y * tileSize, x * -tileSize) - gridOffset;
                GameObject tile = Instantiate(referenceTile, tilePos, Quaternion.identity, transform);

                Tile tileScript = tile.AddComponent<Tile>();
                tileScript.x = x;
                tileScript.y = y;
                grid[x, y] = tileScript;
            }
        }

        Destroy(referenceTile);
        SetTileNeighbors();
        CreateProceduralPath();
        PlacePipes();
    }

    void SetTileNeighbors()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Tile tile = grid[x, y];
                tile.leftNeighbor = x > 0 ? grid[x - 1, y] : null;
                tile.rightNeighbor = x < rows - 1 ? grid[x + 1, y] : null;
                tile.topNeighbor = y < cols - 1 ? grid[x, y + 1] : null;
                tile.bottomNeighbor = y > 0 ? grid[x, y - 1] : null;
            }
        }
    }

    void CreateProceduralPath()
    {
        Tile currentTile = grid[Random.Range(0, rows), Random.Range(0, cols)];
        currentTile.isConnected = true;
        List<Tile> path = new List<Tile> { currentTile };

        while (path.Count < (rows * cols) * 0.9f)
        {
            List<Tile> neighbors = currentTile.GetUnconnectedNeighbors();
            if (neighbors.Count > 0)
            {
                Tile nextTile = neighbors[Random.Range(0, neighbors.Count)];
                nextTile.isConnected = true;
                path.Add(nextTile);
                currentTile = nextTile;
            }
            else
            {
                currentTile = path[Random.Range(0, path.Count)];
            }
        }
    }

    void PlacePipes()
    {
        foreach (Tile tile in grid)
        {
            if (!tile.isConnected) continue;

            if (tile.rightNeighbor != null && tile.rightNeighbor.isConnected)
                CreatePipe(tile, tile.rightNeighbor, verticalPipePrefab);

            if (tile.bottomNeighbor != null && tile.bottomNeighbor.isConnected)
                CreatePipe(tile, tile.bottomNeighbor, horizontalPipePrefab);
        }
    }

    void CreatePipe(Tile start, Tile end, GameObject prefab)
    {
        Vector2 pipePos = (start.transform.position + end.transform.position) / 2;
        Instantiate(prefab, pipePos, Quaternion.identity, transform);
    }

    public List<Tile> GetAllConnectedTiles()
    {
        if (grid == null)
        {
            return new List<Tile>();
        }
        List<Tile> connectedTiles = new List<Tile>();
        foreach (Tile tile in grid)
        {
            if (tile.isConnected) connectedTiles.Add(tile);
        }
        return connectedTiles;
    }
}

public class Tile : MonoBehaviour
{
    public int x, y;
    public Tile leftNeighbor, rightNeighbor, topNeighbor, bottomNeighbor;
    public bool isConnected;

    public List<Tile> GetUnconnectedNeighbors()
    {
        List<Tile> neighbors = new List<Tile>();
        if (leftNeighbor != null && !leftNeighbor.isConnected) neighbors.Add(leftNeighbor);
        if (rightNeighbor != null && !rightNeighbor.isConnected) neighbors.Add(rightNeighbor);
        if (topNeighbor != null && !topNeighbor.isConnected) neighbors.Add(topNeighbor);
        if (bottomNeighbor != null && !bottomNeighbor.isConnected) neighbors.Add(bottomNeighbor);
        return neighbors;
    }
}