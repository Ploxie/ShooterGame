using UnityEngine;

public class Stun : StatusEffect
{
    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Stun; 
    }

    protected override void OnEffect()
    {
        target.ApplyStun(3000);
    }
}