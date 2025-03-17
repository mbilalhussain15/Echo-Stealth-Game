using UnityEngine;
using UnityEngine.UI;  

public class EnergySystem : MonoBehaviour
{
    public int maxEnergy = 100;
    private int currentEnergy;
    public Slider energyBar;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (energyBar == null)
        {
            energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        }
        else
        {
           
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
    }
}