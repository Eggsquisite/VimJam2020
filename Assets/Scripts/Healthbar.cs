using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
    }
}
