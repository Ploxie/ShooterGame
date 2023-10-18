


public class DebilitationModule : EffectModule
{
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