using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public TextMeshProUGUI numbers;

    public Image fill;

    private int max;

    public void SetHealth(int health)
    {
        slider.value = health;
        numbers.text = health.ToString() + " / " + max.ToString();
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        max = maxHealth;
        slider.value = maxHealth;
        numbers.text = maxHealth.ToString() + " / " + max.ToString();
    }

    public void Enemy(bool isEnemy)
    {
        if (isEnemy)
        {
            Color c = Color.red;
            c.a = 0.75f;
            fill.color = c;

        }
        else
        {
            Color c = Color.green;
            c.a = 0.75f;
            fill.color = c;
        }
    }
}
