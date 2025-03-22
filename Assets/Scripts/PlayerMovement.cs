using UnityEngine;
using System.Collections;
using UnityEngine.UI; // UI namespace include karo

public class PlayerMovement : MonoBehaviour
{
    public float moveStep = 1.2f;
    private Vector2 targetPosition;
    private bool isStealthed = false;
    private SpriteRenderer spriteRenderer;
    private GridManager gridManager;
    private Coroutine stealthCoroutine;
    public bool IsStealthed => isStealthed;

    public Text stealthTimerText; // Text component ka reference

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            return;
        }

        StartCoroutine(InitializeAfterGridSetup());
    }

    IEnumerator InitializeAfterGridSetup()
    {
        yield return new WaitUntil(() => gridManager.IsGridInitialized);

        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GetBottomMostConnectedTilePosition();
        targetPosition = transform.position;
        spriteRenderer.color = Color.white;

        // Stealth timer text ko shuru mein hide karo
        if (stealthTimerText != null)
        {
            stealthTimerText.gameObject.SetActive(false);
        }
    }

    Vector2 GetBottomMostConnectedTilePosition()
    {
        Tile bottomTile = null;
        var connectedTiles = gridManager.GetAllConnectedTiles();

        if (connectedTiles.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 gridOffset = new Vector2(
            (gridManager.cols - 1) * gridManager.tileSize / 2,
            (gridManager.rows - 1) * -gridManager.tileSize / 2
        );

        foreach (Tile tile in connectedTiles)
        {
            if (bottomTile == null ||
                tile.x > bottomTile.x ||
                (tile.x == bottomTile.x && tile.y > bottomTile.y))
            {
                bottomTile = tile;
            }
        }

        return new Vector2(
            bottomTile.y * gridManager.tileSize,
            -bottomTile.x * gridManager.tileSize
        ) - gridOffset;
    }

    void Update()
    {
        HandleMovement();
        HandleStealth();
    }

    private bool isMoving = false;

    void HandleMovement()
    {
        if (isMoving) return;

        Vector2 moveDirection = GetInputDirection();
        if (moveDirection == Vector2.zero) return;

        Vector2 newTarget = targetPosition + moveDirection * gridManager.tileSize;

        if (IsPositionValid(newTarget))
        {
            StartCoroutine(MoveToPosition(newTarget));
        }
    }

    IEnumerator MoveToPosition(Vector2 newTarget)
    {
        isMoving = true;
        while (Vector2.Distance(transform.position, newTarget) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, newTarget, moveStep * 10f * Time.deltaTime);
            yield return null;
        }
        transform.position = newTarget;
        targetPosition = newTarget;
        isMoving = false;
    }

    Vector2 GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return Vector2.right;
        return Vector2.zero;
    }

    bool IsPositionValid(Vector2 position)
    {
        Vector2 gridOffset = new Vector2(
            (gridManager.cols - 1) * gridManager.tileSize / 2,
            (gridManager.rows - 1) * -gridManager.tileSize / 2
        );

        Vector2 relativePos = position + gridOffset;
        int x = Mathf.FloorToInt(relativePos.y / -gridManager.tileSize);
        int y = Mathf.FloorToInt(relativePos.x / gridManager.tileSize);

        if (x >= 0 && x < gridManager.rows &&
            y >= 0 && y < gridManager.cols &&
            gridManager.grid[x, y].isConnected)
        {
            return true;
        }
        return false;
    }

    void HandleStealth()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isStealthed)
        {
            isStealthed = true;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            stealthCoroutine = StartCoroutine(DeactivateStealth());
            StartCoroutine(UpdateStealthTimerUI()); // Timer UI update start karo
        }
    }

    IEnumerator DeactivateStealth()
    {
        yield return new WaitForSeconds(6f);
        isStealthed = false;
        spriteRenderer.color = Color.white;
        stealthCoroutine = null;

        // Timer text ko hide karo
        if (stealthTimerText != null)
        {
            stealthTimerText.gameObject.SetActive(false);
        }
    }

    IEnumerator UpdateStealthTimerUI()
    {
        if (stealthTimerText != null)
        {
            stealthTimerText.gameObject.SetActive(true); // Timer text ko show karo
        }

        int timeLeft = 6;
        while (timeLeft > 0)
        {
            if (stealthTimerText != null)
            {
                stealthTimerText.text = "Stealth Timer: " + timeLeft; // Timer update karo
            }
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        // Timer khatam hone par text ko hide karo
        if (stealthTimerText != null)
        {
            stealthTimerText.gameObject.SetActive(false);
        }
    }
}