

using System;

public class ModuleGenerator
{
    public static M CreateWeaponModule<M>() where M : WeaponModule, new()
    {
        return new();
    }

    public static M CreateEffectModule<M>(EffectStats stats) where M : EffectModule, new()
    {
        M module = new()
        {
            BlueprintStats = stats
        };
        return module;
    }

    public static M CreateBulletModule<M>() where M : BulletModule, new()
    {
        return new();
    }
}