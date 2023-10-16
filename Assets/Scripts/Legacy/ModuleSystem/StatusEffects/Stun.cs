using UnityEngine;

public class Stun : StatusEffect
{
    public Stun(StatusEffectData data) : base(data) { }

    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Stun; 
    }

    protected override void OnEffect()
    {
        target.ApplyStun(effectData.Duration * effectData.Potency);
    }
}