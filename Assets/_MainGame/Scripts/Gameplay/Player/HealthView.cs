using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _MainGame.Scripts.Gameplay.Player
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthTxt;
        [SerializeField] private Image healthImg;
        public void SetHealth(float currentHealth, float maxHealth)
        {
            healthTxt.SetText($"{currentHealth}/{maxHealth}");
            healthImg.fillAmount = currentHealth / maxHealth;
        }
    }
}