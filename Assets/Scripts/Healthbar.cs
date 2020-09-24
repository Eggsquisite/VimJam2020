using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
    }
}
