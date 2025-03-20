using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using UnityEngine.SceneManagement;


public class StoryLog : MonoBehaviour
{

    public StoryData storyData;
    private bool isNear = false;
    private bool isCollected = false; 
    public GameObject messagePanel;
    [SerializeField] public Text messageText;
    [SerializeField] public Button continueButton;
    private int currentMessageIndex = 0;
    private static StoryLog currentFragment;
    private static int totalFragmentsPlaced = 0;

    
    public static void ResetFragmentCounter()
    {
        totalFragmentsPlaced = 0;
    }

    void Start()
    {

        messagePanel.SetActive(false);


        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ContinueGame);
        }
        else
        {
            
        }

        if (transform.parent == null && totalFragmentsPlaced < 3)
        {
            PlaceDataFragments();
        }

    }

    void PlaceDataFragments()
    {
        GridManager gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            return;
        }

        ExitNode exitNode = FindFirstObjectByType<ExitNode>();
        if (exitNode == null)
        {
           
            return;
        }
        else
        {
           
        }
        int exitNodeCol = exitNode.exitNodeColumn;

        List<Vector2> validPositions = new List<Vector2>();


        int maxRows = Mathf.Min(4, gridManager.rows);

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < gridManager.cols; col++)
            {
                Tile tile = gridManager.grid[row, col];
                if (tile != null && tile.isConnected && col != exitNodeCol)
                {
                    validPositions.Add(tile.transform.position);
                }
            }
        }

        validPositions = ShuffleList(validPositions);
        List<string> shuffledStories = new List<string>(storyData.encryptedMessages);
        shuffledStories = ShuffleStringList(shuffledStories);

        for (int i = 0; i < Mathf.Min(3, validPositions.Count); i++)
        {
            if (totalFragmentsPlaced >= 3) break; 

            GameObject newFragment = Instantiate(gameObject, validPositions[i], Quaternion.identity);
            StoryLog newStoryLog = newFragment.GetComponent<StoryLog>();
            newStoryLog.storyData = storyData;
            //newStoryLog.currentMessageIndex = i;
            newStoryLog.currentMessageIndex = System.Array.IndexOf(storyData.encryptedMessages, shuffledStories[i]);
            newStoryLog.enabled = true; 

            totalFragmentsPlaced++; 
        }

        gameObject.SetActive(false);
    }
    List<string> ShuffleStringList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    List<Vector2> ShuffleList(List<Vector2> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector2 temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    void Update()
    {
        if (isNear && (Input.GetKeyDown(KeyCode.E)) && isCollected)
        {
            ShowMessage();
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0; 
    }

    void ShowMessage()
    {

        if (currentMessageIndex >= 0 && currentMessageIndex < storyData.encryptedMessages.Length)
        {
            messagePanel.SetActive(true);
            messageText.text = "Decryption in progress...\n" + DecodeMessage(storyData.encryptedMessages[currentMessageIndex]);
            currentFragment = this;
        }
        else
        {
          
        }
    }

    public void ContinueGame()
    {
        messagePanel.SetActive(false);
        Time.timeScale = 1; 
                         
        if (currentFragment != null)
        {
            currentFragment.RemoveFragment();
            currentFragment = null; 
        }
    }

    private IEnumerator RemoveFragmentAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        RemoveFragment();
    }

    string DecodeMessage(string binaryCode)
    {
        string[] binaryWords = binaryCode.Split(' ');
        string decodedMessage = "";
        foreach (string word in binaryWords)
        {
            int asciiValue = Convert.ToInt32(word, 2);
            decodedMessage += (char)asciiValue;
        }
        return decodedMessage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isNear = true;
            isCollected = true; 
            FindFirstObjectByType<EndingManager>().CollectDataFragment();
            StartCoroutine(RemoveFragmentAfterDelay(6));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }

    void RemoveFragment()
    {
       
        gameObject.SetActive(false);
    }
}