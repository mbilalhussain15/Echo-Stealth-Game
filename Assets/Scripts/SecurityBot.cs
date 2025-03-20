using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecurityBot : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    private bool isChasing = false;
    private GameManager gameManager;
    private GridManager gridManager;
    private Rigidbody2D rb;

    private Tile currentTile;
    private Tile previousTile;
    private Vector2 targetGridPosition;
    private bool isMoving = false;
    private bool isGameOver = false;
    private float decisionCooldown = 0.5f;
    private float lastDecisionTime;
    private bool isFrozen = false;
    [SerializeField] private int maxRetryAttempts = 3;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            return;
        }

        if (!SnapToValidGridPosition())
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindFirstObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        lastDecisionTime = Time.time;

        
        SecurityNode.OnNodeHacked += FreezeEnemies;
    }
    void OnDestroy()
    { 
        SecurityNode.OnNodeHacked -= FreezeEnemies;
    }

    void FreezeEnemies()
    {
        if (!isFrozen)
        {
            StartCoroutine(FreezeForSeconds(6f));
        }
    }
    IEnumerator FreezeForSeconds(float seconds)
    {
        isFrozen = true;
        rb.linearVelocity = Vector2.zero; 
        yield return new WaitForSeconds(seconds);
        isFrozen = false;
    }
    bool SnapToValidGridPosition()
    {
        List<Tile> connectedTiles = gridManager.GetAllConnectedTiles();
        if (connectedTiles.Count == 0) return false;

        for (int i = 0; i < maxRetryAttempts; i++)
        {
            Tile randomTile = connectedTiles[Random.Range(0, connectedTiles.Count)];
            Vector2 gridOffset = new Vector2(
                (gridManager.cols - 1) * gridManager.tileSize / 2,
                (gridManager.rows - 1) * -gridManager.tileSize / 2
            );
            Vector2 targetPos = new Vector2(
                randomTile.y * gridManager.tileSize,
                -randomTile.x * gridManager.tileSize
            ) - gridOffset;

            if (!IsPositionOccupied(targetPos))
            {
                transform.position = targetPos;
                UpdateCurrentTile();
                return true;
            }
        }
        return false;
    }

    bool IsPositionOccupied(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Enemy"))
                return true;
        }
        return false;
    }

    void UpdateCurrentTile()
    {
        Vector2 gridOffset = new Vector2(
            (gridManager.cols - 1) * gridManager.tileSize / 2,
            (gridManager.rows - 1) * -gridManager.tileSize / 2
        );

        Vector2 relativePos = (Vector2)transform.position + gridOffset;
        int x = Mathf.FloorToInt(relativePos.y / -gridManager.tileSize);
        int y = Mathf.FloorToInt(relativePos.x / gridManager.tileSize);

        if (x >= 0 && x < gridManager.rows && y >= 0 && y < gridManager.cols)
            currentTile = gridManager.grid[x, y];
    }

    void Update()
    {
        if (isGameOver || player == null || isFrozen) return;

        if (Time.time - lastDecisionTime > decisionCooldown)
        {
            if (isChasing) MoveTowardsPlayer();
            else PatrolGrid();
            lastDecisionTime = Time.time;
        }
        HandleStealthDetection();
    }

    void HandleStealthDetection()
    {
        if (player == null) return;
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        bool shouldIgnore = playerScript != null && playerScript.IsStealthed;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), shouldIgnore);
        if (shouldIgnore) StopChasing();
    }

    void PatrolGrid()
    {
        if (!isMoving && currentTile != null)
        {
            List<Tile> validNeighbors = new List<Tile>();
            if (currentTile.leftNeighbor != null && currentTile.leftNeighbor.isConnected) validNeighbors.Add(currentTile.leftNeighbor);
            if (currentTile.rightNeighbor != null && currentTile.rightNeighbor.isConnected) validNeighbors.Add(currentTile.rightNeighbor);
            if (currentTile.topNeighbor != null && currentTile.topNeighbor.isConnected) validNeighbors.Add(currentTile.topNeighbor);
            if (currentTile.bottomNeighbor != null && currentTile.bottomNeighbor.isConnected) validNeighbors.Add(currentTile.bottomNeighbor);

            validNeighbors.Remove(previousTile);

            if (validNeighbors.Count == 0 && previousTile != null)
                validNeighbors.Add(previousTile);

            if (validNeighbors.Count > 0)
            {
                Tile targetTile = validNeighbors[Random.Range(0, validNeighbors.Count)];
                previousTile = currentTile;
                StartCoroutine(MoveToTile(targetTile));
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (!isMoving && currentTile != null)
        {
            List<Tile> path = FindPathToPlayer();
            if (path != null && path.Count > 0)
            {
                previousTile = currentTile;
                StartCoroutine(MoveToTile(path[0]));
            }
        }
    }

    List<Tile> FindPathToPlayer()
    {
        Vector2 targetPos = GetPlayerGridPosition();
        if (targetPos.x < 0 || targetPos.x >= gridManager.rows || targetPos.y < 0 || targetPos.y >= gridManager.cols)
            return null;

        Tile targetTile = gridManager.grid[(int)targetPos.x, (int)targetPos.y];

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Queue<Tile> frontier = new Queue<Tile>();
        HashSet<Tile> visited = new HashSet<Tile>();

        frontier.Enqueue(currentTile);
        visited.Add(currentTile);

        while (frontier.Count > 0)
        {
            Tile current = frontier.Dequeue();
            if (current == targetTile) break;

            foreach (Tile neighbor in GetValidNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                    frontier.Enqueue(neighbor);
                }
            }
        }

        List<Tile> path = new List<Tile>();
        Tile currentPathTile = targetTile;
        while (currentPathTile != null && currentPathTile != currentTile)
        {
            path.Add(currentPathTile);
            cameFrom.TryGetValue(currentPathTile, out currentPathTile);
        }
        path.Reverse();
        return path.Count > 0 ? path : null;
    }

    List<Tile> GetValidNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();
        if (tile.leftNeighbor != null && tile.leftNeighbor.isConnected) neighbors.Add(tile.leftNeighbor);
        if (tile.rightNeighbor != null && tile.rightNeighbor.isConnected) neighbors.Add(tile.rightNeighbor);
        if (tile.topNeighbor != null && tile.topNeighbor.isConnected) neighbors.Add(tile.topNeighbor);
        if (tile.bottomNeighbor != null && tile.bottomNeighbor.isConnected) neighbors.Add(tile.bottomNeighbor);
        return neighbors;
    }

    Vector2 GetPlayerGridPosition()
    {
        Vector2 gridOffset = new Vector2(
            (gridManager.cols - 1) * gridManager.tileSize / 2,
            (gridManager.rows - 1) * -gridManager.tileSize / 2
        );
        Vector2 relativePos = (Vector2)player.position + gridOffset;
        int x = Mathf.FloorToInt(relativePos.y / -gridManager.tileSize);
        int y = Mathf.FloorToInt(relativePos.x / gridManager.tileSize);
        return new Vector2(x, y);
    }

    IEnumerator MoveToTile(Tile targetTile)
    {
        isMoving = true;
        Vector2 gridOffset = new Vector2(
            (gridManager.cols - 1) * gridManager.tileSize / 2,
            (gridManager.rows - 1) * -gridManager.tileSize / 2
        );
        targetGridPosition = new Vector2(
            targetTile.y * gridManager.tileSize,
            -targetTile.x * gridManager.tileSize
        ) - gridOffset;

        while (Vector2.Distance(transform.position, targetGridPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetGridPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetGridPosition;
        currentTile = targetTile;
        isMoving = false;
    }

    void StopChasing()
    {
        isChasing = false;
        rb.linearVelocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;
        if (other.CompareTag("Player") && !other.GetComponent<PlayerMovement>().IsStealthed)
            isChasing = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerScript = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null && playerScript.IsStealthed) return;

            collision.gameObject.SetActive(false);
            isGameOver = true;
            StopChasing();
            if (gameManager != null) gameManager.GameOver();
        }
    }
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int minEnemies = 3;
    [SerializeField] private int maxEnemies = 6;

    void Start()
    {
        int numEnemies = Random.Range(minEnemies, maxEnemies + 1);
        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}