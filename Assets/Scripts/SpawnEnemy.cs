using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum EnemyType
{
    MELEE,
    KAMIKAZE,
    RANGED
}

[System.Serializable]
public struct EnemySpawnData
{
    public EnemyType TypeOfEnemy;
    public GameObject EnemyObject;
}

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnData> Enemies;

    public ModuleID WeaponID;
    public ModuleID EffectID;
    public ModuleID BulletID;

    private void Start()
    {
        SpawnRandomEnemy();
        
    }

    public void SetupMeleeEnemy(GameObject meleeEnemyObject)
    {
        EnemyMelee meleeEnemy = meleeEnemyObject.GetComponent<EnemyMelee>();
        //meleeEnemy.effect = (EffectModule)ModuleRegistry.CreateModuleByID(EffectID);
    }

    public void SetupKamikazeEnemy(GameObject meleeEnemyObject)
    {
        EnemyKamikaze meleeEnemy = meleeEnemyObject.GetComponent<EnemyKamikaze>();
        //meleeEnemy.effect = (EffectModule)ModuleRegistry.CreateModuleByID(EffectID);
    }

    public void SetupRangedEnemy(GameObject rangedEnemyObject)
    {
        EnemyRanged rangedEnemy = rangedEnemyObject.GetComponent<EnemyRanged>();
        //rangedEnemy.ModuleController.LoadModule(ModuleType.WeaponModule, ModuleRegistry.CreateModuleByID(WeaponID));
        //rangedEnemy.ModuleController.LoadModule(ModuleType.EffectModule, ModuleRegistry.CreateModuleByID(EffectID));
        //rangedEnemy.ModuleController.LoadModule(ModuleType.BulletModule, ModuleRegistry.CreateModuleByID(BulletID));
    }

    public void SpawnRandomEnemy()
    {
        if (Enemies.Count <= 0)
            return;

        EnemySpawnData randomEnemy = Enemies[UnityEngine.Random.Range(0, Enemies.Count-1)];
        GameObject instantiatedEnemy = Instantiate(randomEnemy.EnemyObject, transform.position, Quaternion.identity);

        switch (randomEnemy.TypeOfEnemy)
        {
            case EnemyType.MELEE:
                SetupMeleeEnemy(instantiatedEnemy);
                break;
            case EnemyType.KAMIKAZE:
                SetupKamikazeEnemy(instantiatedEnemy);
                break;
            case EnemyType.RANGED:
                SetupRangedEnemy(instantiatedEnemy);
                break;
        }
    }
}
