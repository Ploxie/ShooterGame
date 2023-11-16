using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponID
{
    None,
    Pistol,
    Shotgun,
    SMG,
    AssaultRifle,
    Sniper
}

public enum StatusID
{
    None,
    DamageReceived,
    Radiation,
    Ice,
    Stun,
    Debilitation
}

public enum EffectID
{
    None,
    Piercing,
    Ricochet,
    Explosive,
    Crystal,
    Cluster,
    BlackHole
}

//Please do not refactor me -Hampus
public class ModuleRepresentation : MonoBehaviour
{
    public static Weapon CreateWeapon(WeaponID ID)
    {
        switch (ID)
        {
            case WeaponID.None:
                return null;
            case WeaponID.Pistol:
                return new PistolWeapon();
            case WeaponID.Shotgun:
                return new ShotgunWeapon();
            case WeaponID.SMG:
                return new SMGWeapon();
            case WeaponID.AssaultRifle:
                return new AssaultRifleWeapon();
            case WeaponID.Sniper:
                return new SniperWeapon();
            default:
                return null;
        }
    }

    //Please do not refactor me -Hampus
    public static StatusEffect CreateStatus(StatusID ID)
    {
        switch (ID)
        {
            case StatusID.None:
                return null;
            case StatusID.DamageReceived:
                return new DamageReceivedEffect();
            case StatusID.Radiation:
                return new RadiationEffect();
            case StatusID.Ice:
                return new IceEffect();
            case StatusID.Stun:
                return new StunEffect();
            case StatusID.Debilitation:
                return new DebilitationEffect();
            default:
                return null;
        }
    }

    //Please do not refactor me -Hampus
    public static ProjectileEffect CreateEffect(EffectID ID)
    {
        switch (ID)
        {
            case EffectID.None:
                return null;
            case EffectID.Piercing:
                return new PiercingEffect();
            case EffectID.Ricochet:
                return new RicochetEffect();
            case EffectID.Explosive:
                return new ExplosionEffect();
            case EffectID.Crystal:
                return new CrystalEffect();
            case EffectID.Cluster:
                return new ClusterEffect();
            case EffectID.BlackHole:
                return new BlackHoleEffect();
            default:
                return null;
        }
    }
}
