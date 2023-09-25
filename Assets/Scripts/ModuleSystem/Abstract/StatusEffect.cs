using System;
using UnityEngine;

public enum StatusEffectID
{
    Debilitation,
    Ice,
    Radiation,
    Slug,
    Stun
}

public abstract class StatusEffect
{
    public EffectStats Stats; 
    
    protected Living target;

    private bool activated;
    private double timeStarted;
    private double timeSinceLastEffect;

    //I am very sorry for this shitty quick fix.
    public abstract StatusEffectID GetID();

    public void Activate(Living target)
    {
        if (activated)
            return;

        this.target = target;
        timeStarted = Utils.GetUnixMillis();
        timeSinceLastEffect = Utils.GetUnixMillis();

        OnActivate();
        activated = true;
    }

    public void Reset()
    {
        if (!activated)
            return;

        timeStarted = Utils.GetUnixMillis();

        OnReset();
    }

    public void Tick()
    {
        if (!activated)
            return;

        if (Utils.GetUnixMillis() - timeStarted >= Stats.Duration)
        {
            OnClear();
            return;
        }

        OnTick();
        if (Utils.GetUnixMillis() - timeSinceLastEffect >= Stats.Interval)
        {
            OnEffect();
            timeSinceLastEffect = Utils.GetUnixMillis();
        }
    }

    public void Clear()
    {
        //Destroy here
        OnClear();
    }

    protected virtual void OnActivate() {}
    protected virtual void OnReset() {}
    protected virtual void OnTick() {}
    protected abstract void OnEffect();
    protected virtual void OnClear() {}
}