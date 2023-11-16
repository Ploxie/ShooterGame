using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollectedData
{
    public int BaseBulletDamage;
    public int BaseTotalBulletDamage;
    public int BulletSpeed;
    public int FireRate;

    public int BulletsPerSecond;
    public int BulletsPerMinute;

    public int CombinedDamage;
    public int TotalCombinedDamage;
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

    public WeaponID CurrentWeaponID;
    public StatusID CurrentStatusID;
    public EffectID CurrentEffectID;

    public float MissPercentage;

    // Start is called before the first frame update
    void Start()
    {
        MissPercentage = PlayerPrefs.GetFloat("MissPercentage");
        MissPercentage = Mathf.Round(MissPercentage * 100);
        MissPercentage /= 100;

        CurrentWeaponID = WeaponID.Pistol;
        RunSimulations();
    }

    //Do not refactor -Hampus
    public void Simulate(WeaponID weapon, StatusID status, EffectID effect)
    {
        Debug.Log($"{weapon} {status} {effect}");
    }

    //Do not refactor me, i swear to god 
    //Cubic time complexity isnt a concern here because N is small.
    //-Hampus
    public void RunSimulations()
    {
        for (int weaponID = 1; weaponID <= MAX_WEAPON_IDS; weaponID++)
        {
            for (int statusID = 0; statusID <= MAX_STATUS_IDS; statusID++)
            {
                for (int effectID = 0; effectID <= MAX_EFFECT_IDS; effectID++)
                {
                    Simulate((WeaponID)weaponID, (StatusID)statusID, (EffectID)effectID);
                }
            }
        }
    }

    void Update()
    {
        
    }
}
