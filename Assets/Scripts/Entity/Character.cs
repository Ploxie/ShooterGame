using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.Scripts.Entity
{
    public enum SpecialWeakness
    {
        Dense, Analytic, Intangible, Porous, Unstable, Armored
    }
    [RequireComponent(typeof(Health)), RequireComponent(typeof(Rigidbody))]
    public abstract class Character : MonoBehaviour
    {
        [HideInInspector] public Health Health { get; private set; }

        [SerializeField] protected float MovementSpeed = 2.0f;
        [HideInInspector] public float CurrentMovementSpeed;

        [SerializeField] protected bool isSpecial = false;
        [SerializeField] protected SpecialWeakness weakness;

        private Dictionary<Type, StatusEffect> statusEffects = new();
        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }
        public MeshToMesh VFXGraph;

        protected virtual void Awake()
        {
            Health = GetComponent<Health>();
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponentInChildren<Collider>();
            VFXGraph = GetComponentInChildren<MeshToMesh>();
            CurrentMovementSpeed = MovementSpeed;
        }


        protected virtual void Update()
        {
            float finalMovementSpeed = MovementSpeed;

            // Maybe swap this to the old way: Effect handling itself and not in character?
            // TODO: Tick rate instead
            // Radiation
            {
                RadiationEffect radiationEffect = GetStatusEffect<RadiationEffect>();
                if (radiationEffect != null)
                    Health.TakeDamage(radiationEffect.Damage * Time.deltaTime);
            }

            // Ice 
            {
                IceEffect iceEffect = GetStatusEffect<IceEffect>();
                if (iceEffect != null)
                    finalMovementSpeed *= iceEffect.MovementSpeedMultiplier;
            }
            // Stun
            {
                StunEffect stunEffect = GetStatusEffect<StunEffect>();
                if (stunEffect != null)
                    finalMovementSpeed = 0.0f;
            }

            CurrentMovementSpeed = finalMovementSpeed;

            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                StatusEffect effect = statusEffects.ElementAt(i).Value;
                effect.Duration -= Time.deltaTime;

                if (effect.Duration <= 0.0f)
                    RemoveStatusEffect(effect);
            }
        }

        public void AddStatusEffect(StatusEffect effect)
        {
            playEffect(effect);
            statusEffects[effect.GetType()] = effect;
            Debug.Log("Status Effect Added: " + effect.GetType());
        }

        private void RemoveStatusEffect(StatusEffect effect)
        {
            stopEffect(effect);
            statusEffects.Remove(effect.GetType());
            Debug.Log("Status Effect Removed: " + effect.GetType());
        }

        private void playEffect(StatusEffect effectPlay)
        {
            StatusEffect value;
            if (effectPlay.GetType() == typeof(RadiationEffect))
            {
                VFXGraph.VFXGraph.SendEvent("OnRadioStop");
                VFXGraph.VFXGraph.SendEvent("OnRadioPlay");
            }
            if (effectPlay.GetType() == typeof(StunEffect))
            {
                VFXGraph.VFXGraph.SendEvent("OnHazardStop");
                VFXGraph.VFXGraph.SendEvent("OnHazardPlay");
            }
            if (effectPlay.GetType() == typeof(IceEffect))
            {
                VFXGraph.VFXGraph.SendEvent("OnIceStop");
                VFXGraph.VFXGraph.SendEvent("OnIcePlay");
            }
        }

        private void stopEffect(StatusEffect effectPlay)
        {
            if (effectPlay.GetType() == typeof(RadiationEffect))
            {
                VFXGraph.VFXGraph.SendEvent("OnRadioStop");
            }
            if (effectPlay.GetType() == typeof(StunEffect))
            {
                VFXGraph.VFXGraph.SendEvent("OnHazardStop");
            }
            if (effectPlay.GetType() == typeof(IceEffect))
            {
                VFXGraph.VFXGraph.SendEvent("OnIceStop");
            }
        }

        public T GetStatusEffect<T>() where T : StatusEffect
        {
            StatusEffect value;
            if (statusEffects.TryGetValue(typeof(T), out value))
            {
                return (T)value;
            }
            return null;
        }

        public virtual void OnHit(float damage, ProjectileEffect projectileEffect, params StatusEffect[] statusEffects)
        {
            int damageWeaknessMultiplier = 1;
            foreach (var effect in statusEffects)
            {
                AddStatusEffect(effect);
            }
            if (isSpecial)
            {
                switch (projectileEffect)
                {
                    case RicochetEffect:
                        if (weakness == SpecialWeakness.Dense)
                            damageWeaknessMultiplier = 5;
                        break;
                    case CrystalEffect:
                        if (weakness == SpecialWeakness.Analytic)
                            damageWeaknessMultiplier = 5;
                        break;
                    case ClusterEffect:
                        if (weakness == SpecialWeakness.Intangible)
                            damageWeaknessMultiplier = 5;
                        break;
                    case ExplosionEffect:
                        if (weakness == SpecialWeakness.Porous)
                            damageWeaknessMultiplier = 5;
                        break;
                    case BlackHoleEffect:
                        if (weakness == SpecialWeakness.Unstable)
                            damageWeaknessMultiplier = 5;
                        break;
                    case PiercingEffect:
                        if (weakness == SpecialWeakness.Armored)
                            damageWeaknessMultiplier = 5;
                        break;

                }
            }
            Health.TakeDamage(damage * damageWeaknessMultiplier);
        }
        public virtual void SetSpecial(SpecialWeakness weakness)
        {
            isSpecial = true;
            this.weakness = weakness;
        }
    }
}
