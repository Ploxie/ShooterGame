

public class RadiationModule : EffectModule
{
    public const int DURABILITY = 2000;

    public RadiationModule()
    {
        this.remainingUses = DURABILITY;
    }

    public override StatusEffectData GetData()
    {
        return new StatusEffectData()
        {
            Duration = 6000,
            Interval = 1000,
            Potency = 1
        };
    }

    public override StatusEffect GetStatusEffect()
    {
        return new Radiation(GetData());
    }
}