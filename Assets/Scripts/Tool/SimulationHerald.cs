using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

public enum DamageRecipient
{
    Player,
    Enemy
}

public enum SimEnemy
{
    Kamikaze,
    Melee,
    Ranged
}

public struct SimEnemyMetadata
{
    public int Count;
    public GameObject Prefab;
    public List<Vector3> Positions;
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

    public Player Player;

    private HashSet<WeaponID> disabledWeapons;
    private HashSet<StatusID> disabledEffects;
    private HashSet<EffectID> disabledBullets;

    private int weaponID;
    private int statusID;
    private int effectID;
    private double simTime;
    private bool simulating;

    private float enemyDamageAccumulator;
    private float playerDamageAccumulator;

    private string fileName;

    public List<GameObject> enemies = new List<GameObject>();

    private Dictionary<SimEnemy, SimEnemyMetadata> simEnemies;

    private List<Enemy> spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        disabledWeapons = new HashSet<WeaponID>();
        disabledEffects = new HashSet<StatusID>();
        disabledBullets = new HashSet<EffectID>();

        MissPercentage = PlayerPrefs.GetFloat("MissPercentage");
        MissPercentage = Mathf.Round(MissPercentage * 100);
        MissPercentage /= 100;

        simEnemies = new Dictionary<SimEnemy, SimEnemyMetadata>();
        spawnedEnemies = new List<Enemy>();

        ResultsDatabase = new Dictionary<int, CollectedData>();

        simTime = Utils.GetUnixMillis();
    }

    private void RegisterEnemyDamage(float damage)
    {
        RegisterDamage(DamageRecipient.Enemy, damage);
    }

    private void RegisterPlayerDamage(float damage)
    {
        RegisterDamage(DamageRecipient.Player, damage);
    }

    public void RegisterDamage(DamageRecipient recipient, float damage)
    {
        switch (recipient)
        {
            case DamageRecipient.Player:
                playerDamageAccumulator += damage;
                break;
            case DamageRecipient.Enemy:
                enemyDamageAccumulator += damage;
                break;
        }
        
    }

    public void RegisterEnemy(SimEnemy type, int prefabID, Vector3 position)
    {
        if (!simEnemies.ContainsKey(type))
        {
            SimEnemyMetadata metadata = new SimEnemyMetadata();
            metadata.Prefab = enemies[prefabID];
            metadata.Positions = new List<Vector3>();
            simEnemies.Add(type, metadata);
        }

        SimEnemyMetadata meta = simEnemies[type];
        meta.Count++;
        meta.Positions.Add(position);
        simEnemies[type] = meta;
        Console.WriteLine("Test");

        //enemy.Health.OnDamageTaken += RegisterEnemyDamage;
        //spawnedEnemies.Add(enemy);
    }

    public void ToggleModule(ModuleType type, int moduleID)
    {
        switch (type)
        {
            case ModuleType.Weapon:
                WeaponID castWeaponID = (WeaponID)moduleID;
                
                if (disabledWeapons.Contains(castWeaponID))
                    disabledWeapons.Remove(castWeaponID);
                else
                    disabledWeapons.Add(castWeaponID);

                break;
            case ModuleType.Effect:
                StatusID castEffectID = (StatusID)moduleID;
                
                if (disabledEffects.Contains(castEffectID))
                    disabledEffects.Remove(castEffectID);
                else
                    disabledEffects.Add(castEffectID);

                break;
            case ModuleType.Bullet:
                EffectID castBulletID = (EffectID)moduleID;

                if (disabledBullets.Contains(castBulletID))
                    disabledBullets.Remove(castBulletID);
                else
                    disabledBullets.Add(castBulletID);
                break;
            default:
                return;
        }
    }


    //Do not refactor me, i swear to god 
    //Cubic time complexity isnt a concern here because N is small.
    //-Hampus
    public void RunSimulations()
    {
        if (simulating)
            return;

        simulating = true;
        weaponID = 1;
        statusID = 0;
        effectID = 0;
        AnimationController.Fire = true;

        enemyDamageAccumulator = 0;
        playerDamageAccumulator = 0;

        Player.Health.OnDamageTaken += RegisterPlayerDamage;
        
        fileName = $"{DateTime.Now.ToString("yyyy-MM-dd_HH.mm")}.simdata.txt";
        Debug.Log(fileName);
        Gun.ApplyModule(ModuleRepresentation.CreateWeapon((WeaponID)weaponID));
        Gun.ApplyModule(ModuleRepresentation.CreateStatus((StatusID)statusID));
        Gun.ApplyModule(ModuleRepresentation.CreateEffect((EffectID)effectID));

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        foreach (Enemy enemy in spawnedEnemies)
        {
            enemy.Health.OnDamageTaken -= RegisterEnemyDamage;
        }
        spawnedEnemies.Clear();

        foreach (KeyValuePair<SimEnemy, SimEnemyMetadata> pair in simEnemies)
        {
            for (int i = 0; i < pair.Value.Count; i++)
            {
                GameObject enemyObject = Instantiate(pair.Value.Prefab, pair.Value.Positions[i], Quaternion.identity);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemy.Health.OnDamageTaken += RegisterEnemyDamage;
                enemy.SimulationEnabled = true;
                enemy.Herald = this;
                spawnedEnemies.Add(enemy);
            }
        }

        simTime = Utils.GetUnixMillis();
    }

    private void LogData(double timeDiff, int weaponID, int statusID, int effectID)
    {
        int key = weaponID * 100 + statusID * 10 + effectID;

        if (ResultsDatabase.ContainsKey(key))
            return;

        Weapon weaponModule = ModuleRepresentation.CreateWeapon((WeaponID)weaponID);
        StatusEffect statusModule = ModuleRepresentation.CreateStatus((StatusID)statusID);
        ProjectileEffect effectModule = ModuleRepresentation.CreateEffect((EffectID)effectID);

        CollectedData data = new CollectedData();
        data.BaseBulletDamage = weaponModule.Data.Damage;
        data.TotalBaseBulletDamage = data.BaseBulletDamage * weaponModule.Data.LaunchAngles.Length;

        StatusID castStatusID = (StatusID)statusID;
        if (castStatusID == StatusID.Radiation)
        {
            RadiationEffect radiation = (RadiationEffect)statusModule;
            data.CombinedBulletDamage = data.BaseBulletDamage + radiation.Data.Damage;
            data.TotalCombinedBulletDamage = data.CombinedBulletDamage * weaponModule.Data.LaunchAngles.Length;

            data.CombinedBulletDamageLifetime = data.CombinedBulletDamage * radiation.Data.Duration;
            data.TotalCombinedBulletDamageLifetime = data.CombinedBulletDamageLifetime * weaponModule.Data.LaunchAngles.Length;
        }
        else if (castStatusID == StatusID.DamageReceived)
        {
            DamageReceivedEffect damageReceived = (DamageReceivedEffect)statusModule;
            data.CombinedBulletDamage = data.BaseBulletDamage * damageReceived.Data.DamageMultiplier;
            data.TotalCombinedBulletDamage = data.CombinedBulletDamage * weaponModule.Data.LaunchAngles.Length;

            data.CombinedBulletDamageLifetime = data.CombinedBulletDamage * damageReceived.Data.Duration;
            data.TotalCombinedBulletDamageLifetime = data.CombinedBulletDamageLifetime * weaponModule.Data.LaunchAngles.Length;
        }
        else
        {
            data.CombinedBulletDamage = data.BaseBulletDamage;
            data.TotalCombinedBulletDamage = data.CombinedBulletDamage * weaponModule.Data.LaunchAngles.Length;

            data.CombinedBulletDamageLifetime = data.CombinedBulletDamage;
            data.TotalCombinedBulletDamageLifetime = data.CombinedBulletDamageLifetime;
        }

        EffectID castEffectID = (EffectID)effectID;
        if (castEffectID == EffectID.Cluster)
        {
            ClusterEffect clusterEffect = (ClusterEffect)effectModule;
            Projectile clusterProjectile = clusterEffect.CreateProjectile(Vector2.zero);
            ClusterProjectile cluster = clusterProjectile.GetComponent<ClusterProjectile>();
            data.TotalCombinedBulletDamage += data.CombinedBulletDamage * cluster.Data.ProjectileCount;
            data.TotalCombinedBulletDamageLifetime += data.CombinedBulletDamageLifetime * cluster.Data.ProjectileCount;
        }
        else if (castEffectID == EffectID.Crystal)
        {
            data.TotalCombinedBulletDamage += data.CombinedBulletDamage * 2;
            data.TotalCombinedBulletDamageLifetime += data.CombinedBulletDamageLifetime * 2;
        }
        else if (castEffectID == EffectID.Explosive)
        {
            data.TotalCombinedBulletDamage += data.CombinedBulletDamage * 3;
            data.TotalCombinedBulletDamageLifetime += data.TotalCombinedBulletDamage * 3;
        }    

        data.TheoreticalDamagePerSecond = data.TotalCombinedBulletDamage * weaponModule.Data.FireRate;

        data.BulletSpeed = weaponModule.Data.ProjectileSpeed;
        data.FireRate = weaponModule.Data.FireRate;

        data.BulletsPerSecond = weaponModule.Data.FireRate * weaponModule.Data.LaunchAngles.Length;
        data.BulletsPerMinute = data.BulletsPerSecond * 60;

        data.DamagePerSecond = enemyDamageAccumulator / (timeDiff / 1000d);

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
                    data.DamageTakenPerSecond += ranged.Gun.Weapon.Data.Damage * ranged.Gun.Weapon.Data.LaunchAngles.Length *
                        ranged.Gun.Weapon.Data.FireRate;
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

        double timeDiff = Utils.GetUnixMillis() - simTime;
        if (timeDiff >= SimulationTime)
        {
            Debug.Log($"{(WeaponID)weaponID} {(StatusID)statusID} {(EffectID)effectID} complete.");
            Projectile[] bullets = GameObject.FindObjectsOfType<Projectile>();
            foreach (Projectile proj in bullets)
                Destroy(proj.gameObject);

            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }


            LogData(timeDiff, weaponID, statusID, effectID);

            foreach (Enemy enemy in spawnedEnemies)
            {
                enemy.Health.OnDamageTaken -= RegisterEnemyDamage;
            }
            spawnedEnemies.Clear();

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

            //if (disabledWeapons.Contains((WeaponID)weaponID) || disabledEffects.Contains((StatusID)statusID) || disabledBullets.Contains((EffectID)effectID))
            //    return;

            Gun.ApplyModule(ModuleRepresentation.CreateWeapon((WeaponID)weaponID));
            Gun.ApplyModule(ModuleRepresentation.CreateStatus((StatusID)statusID));
            Gun.ApplyModule(ModuleRepresentation.CreateEffect((EffectID)effectID));

            enemyDamageAccumulator = 0;
            playerDamageAccumulator = 0;


            foreach (KeyValuePair<SimEnemy, SimEnemyMetadata> pair in simEnemies)
            {
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    GameObject enemyObject = Instantiate(pair.Value.Prefab, pair.Value.Positions[i], Quaternion.identity);
                    Enemy enemy = enemyObject.GetComponent<Enemy>();
                    enemy.Health.OnDamageTaken += RegisterEnemyDamage;
                    enemy.SimulationEnabled = true;
                    enemy.Herald = this;
                    spawnedEnemies.Add(enemy);
                }
            }

            simTime = Utils.GetUnixMillis();
        }
            
        Gun.Shoot();
    }
}
