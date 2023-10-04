

public class SlugModule : EffectModule
{
    public const int DURABILITY = 1500;

    public SlugModule()
    {
        this.remainingUses = DURABILITY;
    }

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