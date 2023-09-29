using UnityEngine;

public class Ice : StatusEffect
{
    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Ice; 
    }

    protected override void OnEffect()
    {
        target.MovementSpeedMultiplier = 0.5f;
    }

    protected override void OnClear()
    {
        target.MovementSpeedMultiplier = 1.0f;
    }
}