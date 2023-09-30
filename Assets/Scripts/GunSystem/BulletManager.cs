using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelGeneration;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private GameObject BulletPrefab;
    private ModuleController moduleController;

    private void Awake()
    {
        BulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile");
        moduleController = GameObject.FindObjectOfType<ModuleController>();
    }

    public GameObject RequestBullet(Vector3 position, Quaternion quaternion, bool applyStatusEffect = true, bool applyBulletEffect = true)
    {
        WeaponData data = moduleController.GetWeaponData();

        GameObject bullet = Instantiate(BulletPrefab, position, quaternion);
        Projectile projectileComponent = bullet.GetComponent<Projectile>();
        projectileComponent.Damage = data.Damage;

        StatusEffect statusEffect = moduleController.GetStatusEffect();
        if (applyStatusEffect && statusEffect != null)
            projectileComponent.AddStatusEffect(statusEffect);

        BulletEffect bulletEffect = moduleController.GetBulletEffect();
        if (applyBulletEffect && bulletEffect != null)
            projectileComponent.AddBulletEffect(bulletEffect);

        return bullet;
    }
}
