using UnityEngine;

public class Debilitation : StatusEffect
{
    public const float DebilitationMultiplier = 0.5f;

    public Debilitation(StatusEffectData data) : base(data) { }

    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Debilitation; 
    }

    protected override void OnEffect()
    {
        if (!target.Nerfable)
            return;

        target.DamageDealtMultiplier = DebilitationMultiplier / effectData.Potency;
    }

    protected override void OnClear()
    {
        target.DamageDealtMultiplier = 1.0f;
    }
}