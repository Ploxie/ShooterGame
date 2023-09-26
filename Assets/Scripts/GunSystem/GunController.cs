using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] public GameObject bullet;

    [Header("Gun Stats")]
    [SerializeField] private int damage;
    [SerializeField] private int bulletsPerShot;
    //[SerializeField] private float range;
    [SerializeField] private float spread;
    [SerializeField] private float cooldown;
    [SerializeField] private float reloadTime;
    //[SerializeField] private Vector3 bulletVelocity;

    [Header("Magazine Info")]
    [SerializeField] private int bulletsShot;
    [SerializeField] private int magazineSize;
    [SerializeField] private int remainingBullets;

    [Header("Status")]
    [SerializeField] private bool shooting;
    [SerializeField] private bool reloading;
    [SerializeField] private bool readyToShoot;
    //[SerializeField] private bool continuousFire;

    private Rigidbody parentRb;

    private void Awake()
    {
        parentRb = GetComponentInParent<Rigidbody>();
        readyToShoot = true;
        remainingBullets = 1;
    }

    private void Update()
    {
        if (remainingBullets <= 0)
        {
            readyToShoot = false;
            Reload();
        }
    }

    public void Shoot()
    {
        if (readyToShoot && !reloading && remainingBullets > 0)
        {
            readyToShoot = false;

            for (int i = 0; i < bulletsPerShot; i++)
            {
                float x = Random.Range(-spread, spread) / 100.0f;
                float z = Random.Range(-spread, spread) / 100.0f;

                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.Damage = damage;
                bulletComponent.Direction = (parentRb.transform.forward + new Vector3(x, 0f, z)).normalized;
                Instantiate(bullet, this.transform.position, parentRb.transform.rotation);
            }

            Invoke("ResetShot", cooldown);
        }
    }

    public void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        remainingBullets = magazineSize;
        reloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
}
