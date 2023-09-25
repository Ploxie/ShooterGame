using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.UI;
using UnityEngine;

//Everything is public since this is a dummy version that wont be used
public class DummyGun : MonoBehaviour
{
    public GameObject BaseProjectile;

    private ModuleController controller;

    //Cached to avoid unnecessary reallocations
    private WeaponData data;
    private double lastFired;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ModuleController>();
        lastFired = Utils.GetUnixMillis();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fireDirection = transform.rotation * Vector3.forward;
        if (Input.GetKey(KeyCode.Space) && Utils.GetUnixMillis() - lastFired >= data.FireRate)
        {
            data = controller.GetWeaponData();
            foreach (float angle in data.LaunchAngles)
            {
                float angleDeviation = UnityEngine.Random.Range(angle-data.AngleDeviation, angle+data.AngleDeviation);
                Vector3 rotatedFireDirection = Quaternion.AngleAxis(angleDeviation, Vector3.up) * fireDirection;

                GameObject bullet = Instantiate(BaseProjectile);
                bullet.transform.position = transform.position;
                bullet.GetComponent<Rigidbody>().AddRelativeForce(rotatedFireDirection * data.LaunchSpeed);

                Projectile projectile = bullet.GetComponent<Projectile>();
                projectile.Damage = data.Damage;
                controller.ApplyEffects(projectile);

            }
            lastFired = Utils.GetUnixMillis();
        }
    }
}
