using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider Slider;

        private int _maxHealth;
        private int _currentHealth;

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = Mathf.Max(value, 0);
                Slider.maxValue = _maxHealth;
                _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
            }
        }

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Max(value, 0);
                Slider.value = _currentHealth;
            }
        }
    }
}
