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

    public Projectile RequestBullet(GunController controller, GameObject parent, Vector3 position, Quaternion quaternion, bool applyStatusEffect = true, bool applyBulletEffect = true)
    {
        WeaponData data = controller.ModuleController.GetWeaponData();

        GameObject bullet = Instantiate(BulletPrefab, position, quaternion);
        Projectile projectileComponent = bullet.GetComponent<Projectile>();
        projectileComponent.Damage = data.Damage;
        projectileComponent.GunController = controller;
        projectileComponent.Parent = parent;

        StatusEffect statusEffect = controller.ModuleController.GetStatusEffect();
        if (applyStatusEffect && statusEffect != null)
            projectileComponent.AddStatusEffect(statusEffect);

        BulletEffect bulletEffect = controller.ModuleController.GetBulletEffect();
        if (applyBulletEffect && bulletEffect != null)
            projectileComponent.AddBulletEffect(bulletEffect);

        return projectileComponent;

    }
}
