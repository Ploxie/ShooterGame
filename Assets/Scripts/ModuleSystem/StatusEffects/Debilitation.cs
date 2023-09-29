using UnityEngine;

public class Debilitation : StatusEffect
{
    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Debilitation; 
    }

    protected override void OnEffect()
    {
        target.DamageDealtMultiplier = 0.5f;
    }

    protected override void OnClear()
    {
        target.DamageDealtMultiplier = 1.0f;
    }
}