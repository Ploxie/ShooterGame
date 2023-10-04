

public class IceModule : EffectModule
{
    public const int DURABILITY = 2000;

    public IceModule()
    {
        this.remainingUses = DURABILITY;
    }

    public override StatusEffectData GetData()
    {
        return new StatusEffectData()
        {
            Duration = 2000,
            Interval = INSTANT,
            Potency = 1
        };
    }

    public override StatusEffect GetStatusEffect()
    {
        return new Ice(GetData());
    }
}