﻿using UnityEngine;
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
        eneregyBonus.gameObject.SetActive(false); // Start with the text hidden
        gameManager = FindFirstObjectByType<GameManager>();

        if (energyBar == null)
        {
            energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        }
        energyBar.interactable = false;
        SetEnergyBarPosition();

        currentEnergy = maxEnergy;
        energyBar.maxValue = maxEnergy;
        energyBar.value = currentEnergy;
    }

    void SetEnergyBarPosition()
    {
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
            UseEnergy(10);
        }
    }

    public void UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            energyBar.value = currentEnergy;
            ShowEnergyChangeText(-amount); // Show decrease text
            if (currentEnergy <= 0)
            {
                gameManager.GameOver();
            }
        }
        else
        {
            if (currentEnergy <= 0)
            {
                gameManager.GameOver();
            }
        }
    }

    public void RechargeEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        energyBar.value = currentEnergy;
        ShowEnergyChangeText(amount); // Show increase text
    }

    // New method to show energy change text
    private void ShowEnergyChangeText(int amount)
    {
        Debug.Log("ShowEnergyChangeText called with amount: " + amount);

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
        float duration = 1.5f; // Duration for which the text is visible
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }

        // Destroy the text object after fading out
        Destroy(text.gameObject);
    }

    // Coroutine for fade-out effect (for eneregyBonus text)
    private IEnumerator FadeOutEnergyBonusText()
    {
        float duration = 1.5f; // Duration for which the text is visible
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            eneregyBonus.color = new Color(eneregyBonus.color.r, eneregyBonus.color.g, eneregyBonus.color.b, alpha);
            yield return null;
        }

        // Hide the text after fading out
        eneregyBonus.gameObject.SetActive(false);
    }
}