using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
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
    
    public int LivingID;
    public float Health;
    public float MovementSpeed;
    public float Damage;

    public float DamageTakenMultiplier = 1;
    public float DamageDealtMultiplier = 1;
    public float MovementSpeedMultiplier = 1;

    public bool Stunned;
    public int StunDuration;
    
    public Dictionary<StatusEffectID, StatusEffect> StatusEffects;
    protected double stunStarted;

    public virtual void Awake()
    {
        LivingID = NextIDPointer++;
        StatusEffects = new Dictionary<StatusEffectID, StatusEffect>();
        Health = MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage * DamageTakenMultiplier;
        OnHealthChangedEvent.Raise(this, Health);
        OnDamage();
    }

    public virtual void ApplyStun(int duration)
    {
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
            OnDeathEvent.Raise(this, 0);
    }

    protected virtual void OnUpdate() {}
    protected virtual void OnDamage() {}
    protected virtual void OnStun() {}
    protected virtual void OnStunEnd() {}
    protected virtual void OnEffectAdded() {}
}