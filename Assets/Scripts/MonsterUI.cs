using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI titleText;
  [SerializeField] private Slider healthSlider;
  [SerializeField] private TextMeshProUGUI healthText;
  
  public void UpdateTitle(string newTitle)
  {
    titleText.SetText(newTitle);
  }
  
  public void UpdateHealth(float currentHealth, float maxHealth)
  {
    healthSlider.maxValue = maxHealth;
    healthSlider.value = currentHealth;
    string newHealthText = $"{currentHealth:F1}/{maxHealth:F1}";
    healthText.SetText(newHealthText);
  }
}
