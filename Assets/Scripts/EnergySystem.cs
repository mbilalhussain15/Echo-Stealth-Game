using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergySystem : MonoBehaviour
{
    public int maxEnergy = 100;
    private int currentEnergy;
    public Slider energyBar;
    private GameManager gameManager;

    // New variables for energy change text
    public GameObject energyChangeTextPrefab; // Assign the prefab in the inspector
    public Transform energyTextSpawnPoint;   // Assign a UI position in the inspector
    [SerializeField] public Text eneregyBonus; // Assign the UI Text in the inspector

    void Start()
    {
        Debug.Log("EnergySystem: Start method called.");

        eneregyBonus.gameObject.SetActive(false); // Start with the text hidden
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("EnergySystem: GameManager not found in the scene!");
        }
        else
        {
            Debug.Log("EnergySystem: GameManager found.");
        }

        if (energyBar == null)
        {
            energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
            if (energyBar == null)
            {
                Debug.LogError("EnergySystem: EnergyBar not found in the scene!");
            }
            else
            {
                Debug.Log("EnergySystem: EnergyBar found.");
            }
        }

        energyBar.interactable = false;
        SetEnergyBarPosition();

        currentEnergy = maxEnergy;
        energyBar.maxValue = maxEnergy;
        energyBar.value = currentEnergy;

        Debug.Log("EnergySystem: Initialized with maxEnergy = " + maxEnergy + ", currentEnergy = " + currentEnergy);
    }

    void SetEnergyBarPosition()
    {
        Debug.Log("EnergySystem: Setting energy bar position.");
        RectTransform energyBarRect = energyBar.GetComponent<RectTransform>();
        energyBarRect.anchorMin = new Vector2(1, 1);
        energyBarRect.anchorMax = new Vector2(1, 1);
        energyBarRect.pivot = new Vector2(1, 1);
        energyBarRect.anchoredPosition = new Vector2(-50, 0);
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.Log("EnergySystem: Space key pressed. Attempting to use 10 energy.");
        UseEnergy(10);
    }
}

    public void UseEnergy(int amount)
{
    Debug.Log("Attempting to use " + amount + " energy. Current Energy: " + currentEnergy);

    if (currentEnergy >= amount)
    {
        currentEnergy -= amount;
        energyBar.value = currentEnergy;
        Debug.Log("Used " + amount + " energy. New currentEnergy = " + currentEnergy);
        ShowEnergyChangeText(-amount); // Show decrease text

        // Check if energy is actually 0 after using energy
        if (currentEnergy <= 0)
        {
            currentEnergy = 0; // Ensure energy doesn't go negative
            Debug.Log("Energy reached 0. Calling GameOver.");
            gameManager.GameOver();
        }
    }
    else
    {
        Debug.Log("Not enough energy to perform this action!");
        if (currentEnergy <= 0)
        {
            currentEnergy = 0; // Ensure energy doesn't go negative
            Debug.Log("Energy reached 0. Calling GameOver.");
            gameManager.GameOver();
        }
    }
}

    public void RechargeEnergy(int amount)
    {
        Debug.Log("EnergySystem: RechargeEnergy called with amount = " + amount + ", currentEnergy = " + currentEnergy);

        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        energyBar.value = currentEnergy;
        Debug.Log("EnergySystem: Recharged " + amount + " energy. New currentEnergy = " + currentEnergy);
        ShowEnergyChangeText(amount); // Show increase text
    }

    // New method to show energy change text
    private void ShowEnergyChangeText(int amount)
    {
        Debug.Log("EnergySystem: ShowEnergyChangeText called with amount = " + amount);

        // Update the UI Text with the energy change value
        eneregyBonus.text = (amount > 0) ? $"+{amount}" : $"{amount}";
        eneregyBonus.color = (amount > 0) ? Color.green : Color.red;

        // Show the text
        eneregyBonus.gameObject.SetActive(true);

        // Start the fade-out coroutine for eneregyBonus
        StartCoroutine(FadeOutEnergyBonusText());

        if (energyChangeTextPrefab != null && energyTextSpawnPoint != null)
        {
            GameObject textInstance = Instantiate(energyChangeTextPrefab, energyTextSpawnPoint.position, Quaternion.identity, energyTextSpawnPoint);
            Text textComponent = textInstance.GetComponent<Text>();
            textComponent.text = (amount > 0) ? $"+{amount}" : $"{amount}";
            textComponent.color = (amount > 0) ? Color.green : Color.red;

            // Add fade-out effect for the prefab text
            StartCoroutine(FadeOutText(textComponent));
        }
    }

    // Coroutine for fade-out effect (for prefab text)
    private IEnumerator FadeOutText(Text text)
    {
        Debug.Log("EnergySystem: Starting FadeOutText coroutine.");

        float duration = 1.5f; // Duration for which the text is visible
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }

        Debug.Log("EnergySystem: FadeOutText coroutine completed. Destroying text object.");
        Destroy(text.gameObject);
    }

    // Coroutine for fade-out effect (for eneregyBonus text)
    private IEnumerator FadeOutEnergyBonusText()
    {
        Debug.Log("EnergySystem: Starting FadeOutEnergyBonusText coroutine.");

        float duration = 1.5f; // Duration for which the text is visible
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            eneregyBonus.color = new Color(eneregyBonus.color.r, eneregyBonus.color.g, eneregyBonus.color.b, alpha);
            yield return null;
        }

        Debug.Log("EnergySystem: FadeOutEnergyBonusText coroutine completed. Hiding text.");
        eneregyBonus.gameObject.SetActive(false);
    }
}