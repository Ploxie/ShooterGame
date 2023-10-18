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
    protected Living target;

    private bool activated;
    private double timeStarted;
    private double timeSinceLastEffect;

    protected StatusEffectData effectData;

    //I am very sorry for this shitty quick fix.
    public abstract StatusEffectID GetID();

    protected StatusEffect(StatusEffectData data)
    {
        effectData = data;
    }

    public void Activate(Living target)
    {
        if (activated)
            return;

        this.target = target;
        timeStarted = Utils.GetUnixMillis();
        timeSinceLastEffect = Utils.GetUnixMillis();

        if (effectData.Interval < 0)
            OnEffect();

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

        if (Utils.GetUnixMillis() - timeStarted >= effectData.Duration * effectData.Potency)
        {
            OnClear();
            return;
        }

        OnTick();
        if (effectData.Interval >= 0 && Utils.GetUnixMillis() - timeSinceLastEffect >= effectData.Interval / effectData.Potency)
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