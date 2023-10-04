using System.Collections.Generic;

public class BoltActionModule : WeaponModule
{
    public const int DURABILITY = 4000;

    public BoltActionModule()
    {
        this.remainingUses = DURABILITY;
        TypeOfWeapon = WeaponType.BoltAction;
    }

    public override WeaponData GetData()
    {
        WeaponData data = new WeaponData
        {
            Damage = 10,
            FireRate = 1600,
            LaunchSpeed = 3000,
            LaunchAngles = new List<float>() { 0 },
            AngleDeviation = 0
        };

        return data;
    }
}