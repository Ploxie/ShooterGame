


public class DebilitationModule : EffectModule
{
    public const int DURABILITY = 1000;

    public DebilitationModule()
    {
        this.remainingUses = DURABILITY;
    }

    public override StatusEffectData GetData()
    {
        return new StatusEffectData()
        {
            Duration = 6000,
            Interval = INSTANT,
            Potency = 1
        };
    }

    public override StatusEffect GetStatusEffect()
    {
        return new Debilitation(GetData());
    }
}