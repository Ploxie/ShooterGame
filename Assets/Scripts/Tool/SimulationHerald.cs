using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct CollectedData
{
    public float BaseBulletDamage; //The immediate damage of one bullet
    public float TotalBaseBulletDamage; //The total immediate damage of all bullets

    public float CombinedBulletDamage; //The immediate damage and over time damage of one bullet
    public float TotalCombinedBulletDamage; //The total immediate damage and over time damage of all bullets

    public float CombinedBulletDamageLifetime; //The immediate damage and over time damage of one bullet throughout its lifetime
    public float TotalCombinedBulletDamageLifetime; //The immediate damage and over time damage of all bullets throughout their lifetime

    public float TheoreticalDamagePerSecond; //The immediate and over time theoretical damage per second of all bullets combined

    public float BulletSpeed; //Speed of the bullet
    public float FireRate; //The cooldown in milliseconds between each bullet

    public float BulletsPerSecond; //How many bullets are fired per second
    public float BulletsPerMinute; //How many bullets are fired per minute

    public double DamagePerSecond; //The immediate and over time damage per second of all hits in the simulation combined

    public double DamageTakenPerSecond; //The immediate and over time damage per second that the player would theoretically sustain from the selected enemy composition.

    public double DamageDealtToTakenRatio; //The ratio of damage dealt vs damage taken. 
}

//Please do not refactor me -Hampus
public class SimulationHerald : MonoBehaviour
{
    //The int is an ID number that is made up by the index of each ID that makes up the combination
    //Example: A combination of just a pistol and nothing else would yield ID 100 since the pistol enum
    //has the index 1 and the None enum in both StatusID and EffectID has the index 0.

    //In other words, the ID number uses a simple system
    //The hundreth number is represented by the index of the WeaponID (Ex: Pistol has ID 1)
    //The tenth number is represented by the index of the StatusID (Ex: Ice has ID 3)
    //The last number is represented by the index of the EffectID (Ex: Ricochet has ID 2)
    //These examples would create the ID number 132 using the system outlined above.
    public Dictionary<int, CollectedData> ResultsDatabase;

    public const int MAX_WEAPON_IDS = 5;
    public const int MAX_STATUS_IDS = 5;
    public const int MAX_EFFECT_IDS = 6;

    public float MissPercentage;
    public int SimulationTime;

    public Gun Gun;
    public AnimatorController AnimationController;

    private int weaponID;
    private int statusID;
    private int effectID;
    private double simTime;
    private bool simulating;

    private float damageAccumulator;

    private string fileName;

    private List<Enemy> spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        MissPercentage = PlayerPrefs.GetFloat("MissPercentage");
        MissPercentage = Mathf.Round(MissPercentage * 100);
        MissPercentage /= 100;

        spawnedEnemies = new List<Enemy>();

        simTime = Utils.GetUnixMillis();
    }

    public void RegisterDamage(float damage)
    {
        damageAccumulator += damage;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemy.Health.OnDamageTaken += RegisterDamage;
        spawnedEnemies.Add(enemy);
    }

    //Do not refactor me, i swear to god 
    //Cubic time complexity isnt a concern here because N is small.
    //-Hampus
    public void RunSimulations()
    {
        simulating = true;
        weaponID = 1;
        statusID = 0;
        effectID = 0;
        AnimationController.Fire = true;

        damageAccumulator = 0;

        fileName = $"{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.simdata.txt")}";

        Gun.ApplyModule(ModuleRepresentation.CreateWeapon((WeaponID)weaponID));
        Gun.ApplyModule(ModuleRepresentation.CreateStatus((StatusID)statusID));
        Gun.ApplyModule(ModuleRepresentation.CreateEffect((EffectID)effectID));

        simTime = Utils.GetUnixMillis();
    }

    private void LogData(int weaponID, int statusID, int effectID)
    {
        int key = weaponID * 100 + statusID * 10 + effectID;

        if (ResultsDatabase.ContainsKey(key))
            return;

        Weapon weaponModule = ModuleRepresentation.CreateWeapon((WeaponID)weaponID);
        StatusEffect statusModule = ModuleRepresentation.CreateStatus((StatusID)statusID);
        ProjectileEffect effectModule = ModuleRepresentation.CreateEffect((EffectID)effectID);

        CollectedData data = new CollectedData();
        data.BaseBulletDamage = weaponModule.Damage;
        data.TotalBaseBulletDamage = data.BaseBulletDamage * weaponModule.LaunchAngles.Length;

        if ((StatusID)statusID == StatusID.Radiation)
        {
            RadiationEffect radiation = (RadiationEffect)statusModule;
            data.CombinedBulletDamage = data.BaseBulletDamage + radiation.Damage;
            data.TotalCombinedBulletDamage = data.CombinedBulletDamage * weaponModule.LaunchAngles.Length;

            data.CombinedBulletDamageLifetime = data.BaseBulletDamage + radiation.Damage * radiation.Duration;
            data.TotalCombinedBulletDamageLifetime = data.CombinedBulletDamageLifetime * weaponModule.LaunchAngles.Length;
        }

        data.TheoreticalDamagePerSecond = data.TotalCombinedBulletDamage * weaponModule.FireRate;

        data.BulletSpeed = weaponModule.ProjectileSpeed;
        data.FireRate = weaponModule.FireRate;

        data.BulletsPerSecond = weaponModule.FireRate * weaponModule.LaunchAngles.Length;
        data.BulletsPerMinute = data.BulletsPerSecond * 60;

        data.DamagePerSecond = damageAccumulator / (simTime / 1000d);
        foreach (Enemy enemy in spawnedEnemies)
        {
            switch (enemy)
            {
                case EnemyKamikaze kamikaze:
                    data.DamageTakenPerSecond += kamikaze.Damage;
                    break;
                case EnemyMelee melee:
                    data.DamageTakenPerSecond += melee.Damage;
                    break;
                case EnemyRanged ranged:
                    data.DamageTakenPerSecond += ranged.Gun.Weapon.Damage * ranged.Gun.Weapon.LaunchAngles.Length * 
                        ranged.Gun.Weapon.FireRate;
                    break;
            }
        }

        if (data.DamageTakenPerSecond != 0)
            data.DamageDealtToTakenRatio = data.DamagePerSecond / data.DamageTakenPerSecond;

        StreamWriter writer = new StreamWriter(fileName, true);
        writer.WriteLine("DATA BEGIN");
        writer.WriteLine($"Combination: {(WeaponID)weaponID}-{(StatusID)statusID}-{(EffectID)effectID}");
        writer.WriteLine($"BaseBulletDamage: {data.BaseBulletDamage}");
        writer.WriteLine($"TotalBaseBulletDamage: {data.TotalBaseBulletDamage}");

        writer.WriteLine($"CombinedBulletDamage: {data.CombinedBulletDamage}");
        writer.WriteLine($"TotalCombinedBulletDamage: {data.TotalCombinedBulletDamage}");

        writer.WriteLine($"CombinedBulletDamageLifetime: {data.CombinedBulletDamageLifetime}");
        writer.WriteLine($"TotalCombinedBulletDamageLifetime: {data.TotalCombinedBulletDamageLifetime}");

        writer.WriteLine($"TheoreticalDamagePerSecond: {data.TheoreticalDamagePerSecond}");

        writer.WriteLine($"BulletSpeed: {data.BulletSpeed}");
        writer.WriteLine($"FireRate: {data.FireRate}");

        writer.WriteLine($"BulletsPerSecond: {data.BulletsPerSecond}");
        writer.WriteLine($"BulletsPerMinute: {data.BulletsPerMinute}");

        writer.WriteLine($"DamagePerSecond: {data.DamagePerSecond}");

        writer.WriteLine($"DamageTakenPerSecond: {data.DamageTakenPerSecond}");

        writer.WriteLine($"DamageDealtToTakenRatio: {data.DamageDealtToTakenRatio}");
        writer.WriteLine("DATA END");
        writer.WriteLine();
        writer.Flush();
        writer.Close();
        writer.Dispose();

        ResultsDatabase.Add(key, data);
    }

    void Update()
    {
        if (!simulating)
            return;

        if (Utils.GetUnixMillis() - simTime >= SimulationTime)
        {
            Debug.Log($"{(WeaponID)weaponID} {(StatusID)statusID} {(EffectID)effectID} complete.");
            Projectile[] bullets = GameObject.FindObjectsOfType<Projectile>();
            foreach (Projectile proj in bullets)
                Destroy(proj.gameObject);

            LogData(weaponID, statusID, effectID);

            if (effectID == MAX_EFFECT_IDS + 1)
            {
                simulating = false;
                AnimationController.Fire = false;
                return;
            }

            weaponID++;
            if (weaponID == MAX_WEAPON_IDS + 1)
            {
                weaponID = 1;
                statusID++;
            }

            if (statusID == MAX_STATUS_IDS + 1)
            {
                statusID = 0;
                effectID++;
            }

            Gun.ApplyModule(ModuleRepresentation.CreateWeapon((WeaponID)weaponID));
            Gun.ApplyModule(ModuleRepresentation.CreateStatus((StatusID)statusID));
            Gun.ApplyModule(ModuleRepresentation.CreateEffect((EffectID)effectID));

            damageAccumulator = 0;

            simTime = Utils.GetUnixMillis();
        }

        Gun.Shoot();
    }
}
