

public class SlugModule : EffectModule
{
    public override StatusEffectData GetData()
    {
        return new StatusEffectData()
        {
            Duration = 5000,
            Interval = INSTANT,
            Potency = 1
        };
    }

    public override StatusEffect GetStatusEffect()
    {
        return new Slug(GetData());
    }
}