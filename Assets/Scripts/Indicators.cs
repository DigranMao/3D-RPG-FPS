using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    public Image healthBar, energyBar;
    public float healthAmount = 100, energyAmount = 100;

    public float timeSubtractEnergy = 5, timeAddEnergy = 10;
    public PlayerController player;
    
    void Start()
    {
        healthBar.fillAmount = player.health / 100;
        energyBar.fillAmount = energyAmount / 100;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && energyAmount > 0 && player.moving)
        {
            energyAmount -= 100 / timeSubtractEnergy * Time.deltaTime;
            energyBar.fillAmount = energyAmount / 100;
        }           
        else if(energyAmount < 100)
        {
            energyAmount += 100 / timeAddEnergy * Time.deltaTime;
            energyBar.fillAmount = energyAmount / 100;
        }

        if(player.health >= 0)
            healthBar.fillAmount = player.health / 100;           
    }
}
