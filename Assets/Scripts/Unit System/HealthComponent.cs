using UnityEngine;
using System;

namespace Tactics.UnitSystem
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int currentHealth = 100;
        [SerializeField] private int maxHealth = 100;

        public event Action OnDamageTaken;
        public event Action OnDeath;

        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0) return;

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }

            else
            {
                OnDamageTaken?.Invoke();
            }
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
        public float GetMaxHealth()
        {
            return maxHealth;
        }
        public float GetHealthNormalized()
        {
            return GetCurrentHealth() / GetMaxHealth();
        }
    }
}

