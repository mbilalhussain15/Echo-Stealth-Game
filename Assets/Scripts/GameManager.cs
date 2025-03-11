using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject messagePanel;
    [SerializeField] private Text gameOverText;
    public float gameOverDelay = 2f; 
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject securityNodeObject;
    [SerializeField] private GameObject echoObject;
    [SerializeField] private GameObject securityBotObject;
    [SerializeField] private GameObject exitNodeObject;
    [SerializeField] private GameObject storyLogObject;
    [SerializeField] private GameObject rechargeStationObject;
    [SerializeField] private GameObject energyBarObject;
    void Start()
    {
        messagePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        securityNodeObject.SetActive(true);
        echoObject.SetActive(true);
        securityBotObject.SetActive(true);
        exitNodeObject.SetActive(true);
        storyLogObject.SetActive(true);
        rechargeStationObject.SetActive(true);
        energyBarObject.SetActive(true);
    }

    public void GameOver()
    {
        messagePanel.SetActive(true);
        gameOverText.text = "Player has been killed. Game Over!";
        restartButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    void RestartGame()
    {
        SecurityNode.ResetPlacement(); 
        RechargeStation.usedColumns.Clear();
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
