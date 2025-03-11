using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryLog : MonoBehaviour
{
    public string encryptedMessage = "01101000 01101001 01100100 01100100 01100101 01101110";
    private bool isNear = false;
    public GameObject messagePanel;
    [SerializeField] private Text messageText;

    void Start()
    {
        messagePanel.SetActive(false);
        PlaceInFirstRow();
    }
    void PlaceInFirstRow()
    {
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null) return;

        ExitNode exitNode = FindFirstObjectByType<ExitNode>();
        int exitNodeCol = exitNode.exitNodeColumn; 

        List<Vector2> validPositions = new List<Vector2>();

        for (int col = 0; col < gridManager.cols; col++)
        {
            Tile tile = gridManager.grid[0, col];
            if (tile != null && tile.isConnected && col != exitNodeCol)
            {
                validPositions.Add(tile.transform.position);
            }
        }

        if (validPositions.Count > 0)
        {
            transform.position = validPositions[Random.Range(0, validPositions.Count)];
        }
    }
    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            messagePanel.SetActive(true);
            messageText.text = "Decryption in progress...\n" + DecodeMessage(encryptedMessage);
        }
    }

    string DecodeMessage(string binaryCode)
    {
        return "The system is watching you...";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            FindFirstObjectByType<EndingManager>().CollectDataFragment();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            messagePanel.SetActive(false);
        }
    }
}
