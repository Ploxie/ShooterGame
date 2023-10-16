using UnityEngine;

public class Slug : StatusEffect
{
    public const float SlugMultiplier = 1.5f;

    public Slug(StatusEffectData data) : base(data) { }

    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Slug; 
    }

    protected override void OnEffect()
    {
        if (!target.Nerfable)
            return;

        target.DamageTakenMultiplier = SlugMultiplier * effectData.Potency;
    }

    protected override void OnClear()
    {
        target.DamageTakenMultiplier = 1.0f;
    }
}