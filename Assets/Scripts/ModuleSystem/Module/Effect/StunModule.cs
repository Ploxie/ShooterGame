

public class StunModule : EffectModule
{
    public override StatusEffect GetStatusEffect()
    {
        Stun effect = new()
        {
            Stats = BlueprintStats
        };
        
        return effect;
    }
}