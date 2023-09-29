using UnityEngine;

public class GunController : MonoBehaviour
{
    private ModuleController controller;

    //Cached to avoid unnecessary reallocations
    private WeaponData data;
    private double lastFired;
    private GameObject baseProjectile;

    private GunVisual gunVisual;

    private void Start()
    {
        controller = GetComponent<ModuleController>();
        lastFired = Utils.GetUnixMillis();
        baseProjectile = Resources.Load<GameObject>("Prefabs/Projectile");
        gunVisual = GetComponent<GunVisual>();
    }

    public void Shoot()
    {
        if (Utils.GetUnixMillis() - lastFired < data.FireRate)
            return;

        Vector3 fireDirection = transform.rotation * Vector3.forward;
        data = controller.GetWeaponData();
        foreach (float angle in data.LaunchAngles)
        {
            float angleDeviation = UnityEngine.Random.Range(angle-data.AngleDeviation, angle+data.AngleDeviation);
            Vector3 rotatedFireDirection = Quaternion.AngleAxis(angleDeviation, Vector3.up) * fireDirection;

            GameObject bullet = Instantiate(baseProjectile);
            bullet.transform.position = gunVisual.GetBarrelPosition();
            bullet.GetComponent<Rigidbody>().AddRelativeForce(rotatedFireDirection * data.LaunchSpeed);

            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.Damage = data.Damage;
            controller.ApplyEffects(projectile);
        }

        lastFired = Utils.GetUnixMillis();
    }
}
