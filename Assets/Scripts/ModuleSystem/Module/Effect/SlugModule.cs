

public class SlugModule : EffectModule
{
    public override StatusEffect GetStatusEffect()
    {
        Slug effect = new()
        {
            Stats = BlueprintStats
        };
        
        return effect;
    }
}