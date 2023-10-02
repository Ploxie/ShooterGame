using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public bool Ranged;
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
        meleeEnemy.effect = (EffectModule)ModuleRegistry.CreateModuleByID(EffectID);
    }

    public void SetupRangedEnemy(GameObject rangedEnemyObject)
    {
        EnemyRanged rangedEnemy = rangedEnemyObject.GetComponent<EnemyRanged>();
        rangedEnemy.ModuleController.LoadModule(ModuleType.WeaponModule, ModuleRegistry.CreateModuleByID(WeaponID));
        rangedEnemy.ModuleController.LoadModule(ModuleType.EffectModule, ModuleRegistry.CreateModuleByID(EffectID));
        rangedEnemy.ModuleController.LoadModule(ModuleType.BulletModule, ModuleRegistry.CreateModuleByID(BulletID));
    }

    public void SpawnRandomEnemy()
    {
        Debug.Log("test");

        if (Enemies.Count <= 0)
            return;

        EnemySpawnData randomEnemy = Enemies[Random.Range(0, Enemies.Count-1)];
        GameObject instantiatedEnemy = Instantiate(randomEnemy.EnemyObject, transform.position, Quaternion.identity);

        if (randomEnemy.Ranged)
            SetupRangedEnemy(instantiatedEnemy);
        else
            SetupMeleeEnemy(instantiatedEnemy);
    }
}
