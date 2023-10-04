using System.Collections.Generic;

public class SMGModule : WeaponModule
{
    public SMGModule()
    {
        TypeOfWeapon = WeaponType.SMG;
    }

    public override WeaponData GetData()
    {
        WeaponData data = new WeaponData
        {
            Damage = 10,
            FireRate = 50,
            LaunchSpeed = 1000,
            LaunchAngles = new List<float>() { 0, 11.25f, -11.25f },
            AngleDeviation = 5
        };

        return data;
    }
}