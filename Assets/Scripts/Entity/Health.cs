﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entity
{
    [Serializable, DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        private Character Character;

        // Events
        public event UnityAction<float> OnDamageTaken;
        public event UnityAction<float> OnHealthGained;
        public event UnityAction OnDeath;

        // Properties
        [field: SerializeField] public float CurrentHealth { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float HealthRegen { get; private set; }

        private void Awake()
        {
            Character = GetComponent<Character>();
        }

        private void Update()
        {
            Heal(HealthRegen * Time.deltaTime);
        }

        public void TakeDamage(float amount)
        {
            float totalAmount = amount;
            float damageMultiplier = 1.0f;

            DamageReceivedEffect damageReceivedEffect = Character.GetStatusEffect<DamageReceivedEffect>();
            if(damageReceivedEffect != null)
                damageMultiplier = damageReceivedEffect.DamageMultiplier;

            totalAmount *= damageMultiplier;
            CurrentHealth = Math.Clamp(CurrentHealth - amount, 0.0f, MaxHealth);
            OnDamageTaken?.Invoke(amount);

            if (CurrentHealth <= 0.0f)
                OnDeath?.Invoke();
        }

        public void Heal(float amount)
        {
            CurrentHealth = Math.Clamp(CurrentHealth + amount, 0.0f, MaxHealth);
            OnHealthGained?.Invoke(amount);
        }
    }
}