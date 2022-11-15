using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  public Image HealthBarSprite;

  public void UpdateHealthBar(float maxHealth, float currentHealth)
  {
    float healthPercentage = currentHealth / maxHealth;
    HealthBarSprite.fillAmount = healthPercentage;
  }
}
