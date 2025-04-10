using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
   [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;


    public void SetHealthBar(int currHP, int maxHP = -1)
    {


        if (maxHP > 0) healthBar.maxValue = maxHP;
        else maxHP = (int)healthBar.maxValue;

        healthBar.value = currHP;

        if (healthText != null) healthText.text = currHP + "/" + maxHP + " HP";

    }
}
