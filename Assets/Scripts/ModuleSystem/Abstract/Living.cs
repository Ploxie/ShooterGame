using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public abstract class Living : MonoBehaviour
{
    public static int NextIDPointer = 0;

    public float MaxHealth;
    public float DefaultMovementSpeed;
    public float DefaultDamage;

    public GameEvent OnHealthChangedEvent;
    public GameEvent OnDeathEvent;

    public bool RegenHealth;
    public bool Slowable;
    public bool Nerfable;
    public bool Stunnable;
    public bool Alive;
    
    public int LivingID;
    public float Health;
    public float HealthRegenAmount;
    public double HealthRegenInterval;
    public float MovementSpeed;
    public float Damage;

    public float DamageTakenMultiplier = 1;
    public float DamageDealtMultiplier = 1;
    public float MovementSpeedMultiplier = 1;

    public bool Stunned;
    public float StunDuration;
    
    public Dictionary<StatusEffectID, StatusEffect> StatusEffects;
    protected double stunStarted;
    protected double lastHealthRegen;

    public virtual void Awake()
    {
        LivingID = NextIDPointer++;
        StatusEffects = new Dictionary<StatusEffectID, StatusEffect>();
        lastHealthRegen = Utils.GetUnixMillis();
        Health = MaxHealth;
        Alive = true;
    }

    public virtual void Start()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage * DamageTakenMultiplier;
        OnHealthChangedEvent.Raise(this, Health);
        OnDamage();
    }

    public virtual void ApplyStun(float duration)
    {
        if (!Stunnable)
            return;

        Stunned = true;
        stunStarted = Utils.GetUnixMillis();
        StunDuration = duration;
        OnStun();
    }

    public void AddEffect(StatusEffect effect)
    {
        //Once again, very sorry for this quick fix.
        StatusEffectID effectID = effect.GetID();
        if (StatusEffects.ContainsKey(effectID))
        {
            StatusEffects[effectID].Reset();
            return;
        }
        
        effect.Activate(this);
        StatusEffects.Add(effectID, effect);
    }

    public virtual void Update()
    {
        if (Stunned && Utils.GetUnixMillis() - stunStarted >= StunDuration )
        {
            Stunned = false;
            OnStunEnd();
        }

        MovementSpeed = DefaultMovementSpeed * MovementSpeedMultiplier;
        Damage = DefaultDamage * DamageDealtMultiplier;

        foreach (StatusEffect effect in StatusEffects.Values)
        {
            effect.Tick();
        }

        if (!Stunned)
            OnUpdate();

        if (Health <= 0)
        {
            if (Alive)
            {
                OnDeath();
                OnDeathEvent?.Raise(this, 0);
                Alive = false;
            }
        }

        if (Utils.GetUnixMillis() - lastHealthRegen >= HealthRegenInterval)
        {
            Health += HealthRegenAmount;
            lastHealthRegen = Utils.GetUnixMillis();
        }

        Mathf.Clamp(Health, 0, MaxHealth);
    }

    protected virtual void OnUpdate() {}
    protected virtual void OnDamage() {}
    protected virtual void OnDeath() {}
    protected virtual void OnStun() {}
    protected virtual void OnStunEnd() {}
    protected virtual void OnEffectAdded() {}
}