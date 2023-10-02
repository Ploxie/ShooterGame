using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleID
{
    //Weapon
    WPN_PISTOL,
    WPN_SHOTGUN,
    WPN_BOLT_ACTION,
    WPN_AUTOMATIC,
    WPN_SMG,

    //Effect
    EFF_DEBILITATION,
    EFF_ICE,
    EFF_RADIATION,
    EFF_SLUG,
    EFF_STUN,

    //Bullet
    BUL_BLACK_HOLE,
    BUL_CLUSTER,
    BUL_CRYSTAL,
    BUL_EXPLOSIVE,
    BUL_PIERCING,
    BUL_RICOCHET
}

public static class ModuleRegistry
{
    public static Module CreateModuleByID(ModuleID id)
    {
        //This should not be assigned here. Replace in the future.
        EffectStats stats = new EffectStats
        {
            Duration = 10000,
            Interval = 1000
        };

        switch (id)
        {
            case ModuleID.WPN_PISTOL:
                return ModuleGenerator.CreateWeaponModule<PistolModule>();
            case ModuleID.WPN_SHOTGUN:
                return ModuleGenerator.CreateWeaponModule<ShotgunModule>();
            case ModuleID.WPN_BOLT_ACTION:
                return ModuleGenerator.CreateWeaponModule<BoltActionModule>();
            case ModuleID.WPN_AUTOMATIC:
                return ModuleGenerator.CreateWeaponModule<AutomaticModule>();
            case ModuleID.WPN_SMG:
                return ModuleGenerator.CreateWeaponModule<SMGModule>();

            case ModuleID.EFF_STUN:
                return ModuleGenerator.CreateEffectModule<StunModule>(stats);
            case ModuleID.EFF_DEBILITATION:
                return ModuleGenerator.CreateEffectModule<DebilitationModule>(stats);
            case ModuleID.EFF_ICE:
                return ModuleGenerator.CreateEffectModule<IceModule>(stats);
            case ModuleID.EFF_RADIATION:
                return ModuleGenerator.CreateEffectModule<RadiationModule>(stats);
            case ModuleID.EFF_SLUG:
                return ModuleGenerator.CreateEffectModule<SlugModule>(stats);

            case ModuleID.BUL_CLUSTER:
                return ModuleGenerator.CreateBulletModule<ClusterModule>();
            case ModuleID.BUL_CRYSTAL:
                return ModuleGenerator.CreateBulletModule<CrystalModule>();
            case ModuleID.BUL_BLACK_HOLE:
                return ModuleGenerator.CreateBulletModule<BlackHoleModule>();
            case ModuleID.BUL_EXPLOSIVE:
                return ModuleGenerator.CreateBulletModule<ExplosiveModule>();
            case ModuleID.BUL_PIERCING:
                return ModuleGenerator.CreateBulletModule<PiercingModule>();
            case ModuleID.BUL_RICOCHET:
                return ModuleGenerator.CreateBulletModule<RicochetModule>();

            default:
                return null;
        }
    }
}
