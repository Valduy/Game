using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Game
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private int _maxHealth;
        private int _currentHealth;

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = Mathf.Max(value, 0);
                _slider.maxValue = _maxHealth;
                _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
            }
        }

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Max(value, 0);
                _slider.value = _currentHealth;
            }
        }
    }
}
