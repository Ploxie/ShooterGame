using System.Collections.Generic;

public class BoltActionModule : WeaponModule
{
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