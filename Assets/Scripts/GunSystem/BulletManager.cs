using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelGeneration;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private GameObject BulletPrefab;

    private void Awake()
    {
        BulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }

    public GameObject RequestBullet(GunController controller, ModuleHolder weaponModules, ModuleHolder effectModules, ModuleHolder bulletModules, Vector3 position, Quaternion quaternion, bool applyStatusEffect = true, bool applyBulletEffect = true)
    {
        WeaponData data = controller.ModuleController.GetWeaponData(weaponModules);

        GameObject bullet = Instantiate(BulletPrefab, position, quaternion);
        Projectile projectileComponent = bullet.GetComponent<Projectile>();
        projectileComponent.Damage = data.Damage;
        projectileComponent.GunController = controller;

        StatusEffect statusEffect = controller.ModuleController.GetStatusEffect(effectModules);
        if (applyStatusEffect && statusEffect != null)
            projectileComponent.AddStatusEffect(statusEffect);

        BulletEffect bulletEffect = controller.ModuleController.GetBulletEffect(bulletModules);
        if (applyBulletEffect && bulletEffect != null)
            projectileComponent.AddBulletEffect(bulletEffect);

        return bullet;
    }
}
