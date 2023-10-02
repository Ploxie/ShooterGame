

using System;

public class ModuleGenerator
{
    public static M CreateWeaponModule<M>() where M : WeaponModule, new()
    {
        return new();
    }

    public static M CreateEffectModule<M>() where M : EffectModule, new()
    {
        return new();
    }

    public static M CreateBulletModule<M>() where M : BulletModule, new()
    {
        return new();
    }
}