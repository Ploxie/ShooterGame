using UnityEngine;

public class Radiation : StatusEffect
{
    public const float RadiationDamage = 10;

    public Radiation(StatusEffectData data) : base(data) { }

    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Radiation; 
    }

    protected override void OnEffect()
    {
        target.TakeDamage(RadiationDamage * effectData.Potency);
    }
}