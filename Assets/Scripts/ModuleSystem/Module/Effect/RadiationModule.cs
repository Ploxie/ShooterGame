

public class RadiationModule : EffectModule
{
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