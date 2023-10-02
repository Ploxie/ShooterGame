

public class StunModule : EffectModule
{
    public override StatusEffectData GetData()
    {
        return new StatusEffectData()
        {
            Duration = 1500,
            Interval = INSTANT,
            Potency = 1
        };
    }

    public override StatusEffect GetStatusEffect()
    {
        return new Stun(GetData());
    }
}