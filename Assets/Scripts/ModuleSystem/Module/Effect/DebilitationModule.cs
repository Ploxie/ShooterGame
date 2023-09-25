


public class DebilitationModule : EffectModule
{
    public override StatusEffect GetStatusEffect()
    {
        Debilitation effect = new()
        {
            Stats = BlueprintStats
        };
        
        return effect;
    }
}