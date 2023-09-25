

public class RadiationModule : EffectModule
{
    public override StatusEffect GetStatusEffect()
    {
        Radiation effect = new()
        {
            Stats = BlueprintStats
        };
        
        return effect;
    }
}