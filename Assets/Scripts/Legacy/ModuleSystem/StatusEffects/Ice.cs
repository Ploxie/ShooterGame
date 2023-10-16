using UnityEngine;

public class Ice : StatusEffect
{
    public const float SlowMultiplier = 0.5f;

    public Ice(StatusEffectData data) : base(data) { }

    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Ice; 
    }

    protected override void OnEffect()
    {
        if (!target.Slowable)
            return;

        target.MovementSpeedMultiplier = SlowMultiplier / effectData.Potency;
    }

    protected override void OnClear()
    {
        target.MovementSpeedMultiplier = 1.0f;
    }
}