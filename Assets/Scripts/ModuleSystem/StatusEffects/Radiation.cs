using UnityEngine;

public class Radiation : StatusEffect
{
    public override StatusEffectID GetID() 
    { 
        return StatusEffectID.Radiation; 
    }

    protected override void OnEffect()
    {
        target.TakeDamage(10);          
    }
}