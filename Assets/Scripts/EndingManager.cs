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
        StoryLog.ResetFragmentCounter();
        UpdateHiddenTruthCounter();
        UpdateDataFragmentsText();
    }

   
    public void ResumeGame()
    {
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