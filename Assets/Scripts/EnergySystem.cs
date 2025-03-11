using UnityEngine;
using UnityEngine.UI;  

public class EnergySystem : MonoBehaviour
{
    public int maxEnergy = 100;
    private int currentEnergy;
    public Slider energyBar; 

    void Start()
    {
        if (energyBar == null)
        {
            energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        }
        else
        {
           
        }

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseEnergy(20);
        }
    }

    public void UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            energyBar.value = currentEnergy;
        }
        else
        {
          
        }
    }

    public void RechargeEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        energyBar.value = currentEnergy;
    }
}