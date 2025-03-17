using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    private static int dataFragmentsCollected = 0; 
    private static int totalDataFragments = 3;    

    public GameObject endingPopup;
    public Text endingMessage;    
    public Text Level;            
    public Text DataFragments;    
    [SerializeField] public Button resumeButton;   
    public static int secretEndingCount = 1;
    public static int hiddenTruthCount = 1;

    private void Start()
    {
       
        resumeButton.onClick.AddListener(ResumeGame);

       
        UpdateHiddenTruthCounter();
        UpdateDataFragmentsText();
    }

    
    public bool HasAllFragments()
    {
        return dataFragmentsCollected >= totalDataFragments;
    }

   
    public void CollectDataFragment()
    {
        if (dataFragmentsCollected < totalDataFragments)
        {
            dataFragmentsCollected++;
            UpdateDataFragmentsText(); 
        }
    }

   
    public void TriggerEnding()
    {
        if (HasAllFragments())
        {
            secretEndingCount++; 
            endingMessage.text = $"You have unlocked the hidden truth...\nLevel: {secretEndingCount}";
            resumeButton.gameObject.SetActive(false);
            endingPopup.SetActive(true);
            Time.timeScale = 0;
            hiddenTruthCount++;
            UpdateHiddenTruthCounter();
            StartCoroutine(RestartAfterDelay(4)); 
        }
        else
        {
           
            endingMessage.text = "You escaped, but some secrets remain...";
            resumeButton.gameObject.SetActive(true); 
            endingPopup.SetActive(true);
            Time.timeScale = 0;
        }
    }

    
    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SecurityNode.ResetPlacement();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1; 

        totalDataFragments += 3;

        UpdateHiddenTruthCounter();
        UpdateDataFragmentsText();
    }

   
    public void ResumeGame()
    {
        Debug.Log("ResumeGame() called!");
        endingPopup.SetActive(false);
        Time.timeScale = 1; 
    }

    private void UpdateHiddenTruthCounter()
    {
        if (Level != null)
        {
            Level.text = "Level: " + hiddenTruthCount;
        }
    }

    private void UpdateDataFragmentsText()
    {
        if (DataFragments != null)
        {
            DataFragments.text = $"Data Fragments: {dataFragmentsCollected} / {totalDataFragments}";
        }
    }
}













//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections;

//public class EndingManager : MonoBehaviour
//{
//    private int dataFragmentsCollected = 0;
//    private int totalDataFragments = 3;

//    public GameObject endingPopup; // Assign in Unity Inspector
//    public Text endingMessage;     // Assign in Unity Inspector
//    public Text Level;
//    public Text DataFragments;
//    [SerializeField] public Button resumeButton;    // Assign in Unity Inspector
//    public static int secretEndingCount = 1;
//    public static int hiddenTruthCount = 1;

//    private void Start()
//    {
//        // Add listener for the button click
//        resumeButton.onClick.AddListener(ResumeGame);

//        // Initialize counter text
//        UpdateHiddenTruthCounter();
//        UpdateDataFragmentsText();
//    }
//    // Check if all fragments are collected
//    public bool HasAllFragments()
//    {
//        return dataFragmentsCollected >= totalDataFragments;
//    }

//    // Called when player collects a fragment
//    public void CollectDataFragment()
//    {
//        if (dataFragmentsCollected < totalDataFragments) // Ensure it doesn't exceed totalDataFragments
//        {
//            dataFragmentsCollected++;
//            UpdateDataFragmentsText(); // Update UI text dynamically
//        }
//    }

//    // Called when player reaches exit
//    public void TriggerEnding()
//    {
//        if (HasAllFragments())
//        {
//            secretEndingCount++; // Increment counter
//            endingMessage.text = $"You have unlocked the hidden truth...\nLevel: {secretEndingCount}";
//            //endingMessage.text = "You have unlocked the hidden truth...";
//            resumeButton.gameObject.SetActive(false); // Hide resume button
//            endingPopup.SetActive(true);
//            Time.timeScale = 0;
//            hiddenTruthCount++;
//            UpdateHiddenTruthCounter();
//            StartCoroutine(RestartAfterDelay(4)); // Restart after 4 seconds
//        }
//        else
//        {
//            // Normal Ending
//            endingMessage.text = "You escaped, but some secrets remain...";
//            resumeButton.gameObject.SetActive(true); // Show resume button
//            endingPopup.SetActive(true);
//            Time.timeScale = 0; // Pause game
//        }
//    }

//    // Coroutine to restart the game
//    private IEnumerator RestartAfterDelay(float delay)
//    {
//        yield return new WaitForSecondsRealtime(delay); // Works even when paused
//        SecurityNode.ResetPlacement();
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//        Time.timeScale = 1; // Reset time scale

//        totalDataFragments += 3;



//        UpdateHiddenTruthCounter();
//        UpdateDataFragmentsText();
//    }

//    // Called when resume button is clicked
//    public void ResumeGame()
//    {
//        Debug.Log("ResumeGame() called!");
//        endingPopup.SetActive(false);
//        Time.timeScale = 1; // Unpause game
//    }
//    private void UpdateHiddenTruthCounter()
//    {
//        if (Level != null)
//        {
//            Level.text = "Level: " + hiddenTruthCount;
//        }
//    }
//    private void UpdateDataFragmentsText()
//    {
//        if (DataFragments != null)
//        {
//            DataFragments.text = $"Data Fragments: {dataFragmentsCollected} / {totalDataFragments}";
//        }
//    }
//    //public void CollectDataFragment()
//    //{
//    //    dataFragmentsCollected++;

//    //    if (dataFragmentsCollected >= totalDataFragments)
//    //    {
//    //        UnlockSecretEnding();
//    //    }
//    //}

//    //void UnlockSecretEnding()
//    //{
//    //    SceneManager.LoadScene("SecretEnding");
//    //}

//    //public void NormalExit()
//    //{
//    //    SceneManager.LoadScene("NormalEnding");
//    //}
//}
