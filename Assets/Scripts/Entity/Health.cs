using System;
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
        [field: SerializeField] public float CurrentHealth { get; private set; } = 100;
        [field: SerializeField] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField] public float HealthRegen { get; private set; } = 10;

        public bool IsDead { get; private set; }

        private void Awake()
        {
            Character = GetComponent<Character>();
        }

        private void Update()
        {
            if(!IsDead)
            {
                Heal(HealthRegen * Time.deltaTime);
            }
        }

        public void TakeDamage(float amount)
        {
            if (IsDead)
                return;

            float totalAmount = amount;
            float damageMultiplier = 1.0f;

            DamageReceivedEffect damageReceivedEffect = Character.GetStatusEffect<DamageReceivedEffect>();
            if(damageReceivedEffect != null)
                damageMultiplier = damageReceivedEffect.DamageMultiplier;

            totalAmount *= damageMultiplier;
            CurrentHealth = Math.Clamp(CurrentHealth - amount, 0.0f, MaxHealth);
            OnDamageTaken?.Invoke(amount);

            Vector3 randomness = new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            DamagePopUpGenerator.Instance.CreatePopUp(transform.position + randomness + new Vector3(0, 4, 0), ((int)amount).ToString());      

            if (CurrentHealth <= 0.0f)
            {
                OnDeath?.Invoke();
                IsDead = true;
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth = Math.Clamp(CurrentHealth + amount, 0.0f, MaxHealth);
            OnHealthGained?.Invoke(amount);
        }
        public void Multiply(int multiplier)
        {
            MaxHealth *= multiplier;
            CurrentHealth *= multiplier;
        }
    }
}
