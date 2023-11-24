using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Entity
{
    public class Gun : MonoBehaviour
    {        
        private GunVisual GunVisual { get; set; }
        public StatusEffect StatusEffect { get; set; }
        public ProjectileEffect ProjectileEffect { get; set; }
        public Weapon Weapon { get; set; }
        private Character Parent { get; set; }

        private GameObject muzzleFlash;

        private void Awake()
        {            
            GunVisual = GetComponentInChildren<GunVisual>();
            Parent = GetComponent<Character>();

            Weapon = new PistolWeapon();
            muzzleFlash = Resources.Load<GameObject>("Prefabs/VFX/MuzzelFlash");
        }

        public void Shoot()
        {
            if (!Weapon.CanShoot())
                return;
            GunVisual.PlaySound();
            float damageMultiplier = 1.0f;
            
            DebilitationEffect debilitationEffect = Parent.GetStatusEffect<DebilitationEffect>();
            if (debilitationEffect != null)
                damageMultiplier = debilitationEffect.DamageMultiplier;

            if (gameObject.tag == "Player")
            {
                Player player = Parent as Player;
                if (player.powerUpActive)
                {
                    damageMultiplier = 2.0f;
                }
            }

            foreach(float launchAngle in Weapon.LaunchAngles)
            {
                float angleDeviation = UnityEngine.Random.Range(launchAngle - Weapon.AngleDeviation, launchAngle + Weapon.AngleDeviation);
                Vector3 rotatedFireDirection = Quaternion.AngleAxis(angleDeviation, Vector3.up) * transform.rotation * Vector3.forward;

                Vector3 barrelPosition = GunVisual != null ? GunVisual.GetBarrelPosition() : transform.position;

                GameObject muzzleFlash = Instantiate(this.muzzleFlash, transform);
                muzzleFlash.transform.position = barrelPosition;
                //muzzleFlash.transform.rotation = Quaternion.LookRotation(rotatedFireDirection, Vector3.up);

                Projectile projectile;
                if (ProjectileEffect != null)
                    projectile = ProjectileEffect.CreateProjectile(barrelPosition);
                else
                    projectile = ProjectileEffect.CreateDefaultProjectile(barrelPosition);

                
                projectile = Weapon.ShootProjectile(projectile);
                projectile.Damage *= damageMultiplier;
                projectile.Direction = rotatedFireDirection;
                projectile.tag = Parent.tag;

                if (StatusEffect != null)
                    projectile.StatusEffects.Add(StatusEffect.Copy());
                
            }            
        }

        public void ApplyModule(Module module)
        {
            if (module is StatusEffect statusEffect)
            {
                StatusEffect = statusEffect;
            }
            else if (module is ProjectileEffect projectileEffect)
            {
                ProjectileEffect = projectileEffect;
            }
            else if (module is Weapon weaponModule)
            {
                Weapon = weaponModule;
            }
        }
    }
}
