using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections;

public abstract class Living : MonoBehaviour
{
    public static int NextIDPointer = 0;

    [BoxGroup("Parameters")]
    public float MaxHealth;
    [BoxGroup("Parameters")]
    public float DefaultMovementSpeed;
    [BoxGroup("Parameters")]
    public float DefaultDamage;

    [BoxGroup("Toggles")]
    public bool RegenHealth;
    [BoxGroup("Toggles")]
    public bool Slowable;
    [BoxGroup("Toggles")]
    public bool Nerfable;
    [BoxGroup("Toggles")]
    public bool Stunnable;
    
    [ReadOnly]
    [BoxGroup("Debug")]
    public int LivingID;
    [ReadOnly]
    [BoxGroup("Debug")]
    public float Health;
    [ReadOnly]
    [BoxGroup("Debug")]
    public float MovementSpeed;
    [ReadOnly]
    [BoxGroup("Debug")]
    public float Damage;
    [HorizontalLine(2, EColor.Gray)]
    [ReadOnly]
    [BoxGroup("Debug")]
    public float DamageTakenMultiplier = 1;
    [ReadOnly]
    [BoxGroup("Debug")]
    public float DamageDealtMultiplier = 1;
    [ReadOnly]
    [BoxGroup("Debug")]
    public float MovementSpeedMultiplier = 1;
    [HorizontalLine(2, EColor.Gray)]
    [ReadOnly]
    [BoxGroup("Debug")]
    public bool Stunned;
    [ReadOnly]
    [BoxGroup("Debug")]
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
    }

    protected virtual void OnUpdate() {}
    protected virtual void OnDamage() {}
    protected virtual void OnStun() {}
    protected virtual void OnStunEnd() {}
    protected virtual void OnEffectAdded() {}
}