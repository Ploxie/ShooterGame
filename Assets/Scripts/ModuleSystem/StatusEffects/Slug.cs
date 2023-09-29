using UnityEngine;

public class Slug : StatusEffect
{
    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Slug; 
    }

    protected override void OnEffect()
    {
        target.DamageTakenMultiplier = 1.5f;
    }

    protected override void OnClear()
    {
        target.DamageTakenMultiplier = 1.0f;
    }
}