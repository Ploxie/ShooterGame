

public class IceModule : EffectModule
{
    public override StatusEffect GetStatusEffect()
    {
        Ice effect = new()
        {
            Stats = BlueprintStats
        };
        
        return effect;
    }
}