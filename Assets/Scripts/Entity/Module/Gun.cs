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
    [RequireComponent(typeof(GunVisual))]
    public class Gun : MonoBehaviour
    {        
        private GunVisual GunVisual { get; set; }
        public StatusEffect StatusEffect { get; set; }
        public ProjectileEffect ProjectileEffect { get; set; }
        public Weapon Weapon { get; set; }
        private Character Parent { get; set; }

        private void Awake()
        {
            
            GunVisual = GetComponentInChildren<GunVisual>();
            Parent = GetComponent<Character>();

            StatusEffect = new RadiationEffect();
            ProjectileEffect = new BlackHoleEffect();
            Weapon = new PistolWeapon();
        }

        public void Shoot()
        {
            if (!Weapon.CanShoot())
                return;

            float damageMultiplier = 1.0f;

            DebilitationEffect debilitationEffect = Parent.GetStatusEffect<DebilitationEffect>();
            if (debilitationEffect != null)
                damageMultiplier = debilitationEffect.DamageMultiplier;

            foreach(float launchAngle in Weapon.LaunchAngles)
            {
                float angleDeviation = UnityEngine.Random.Range(launchAngle - Weapon.AngleDeviation, launchAngle + Weapon.AngleDeviation);
                Vector3 rotatedFireDirection = Quaternion.AngleAxis(angleDeviation, Vector3.up) * transform.rotation * Vector3.forward;

                Projectile projectile = ProjectileEffect.CreateProjectile(GunVisual.GetBarrelPosition());
                {
                    projectile = Weapon.ShootProjectile(projectile);
                    projectile.Damage *= damageMultiplier;
                    projectile.Direction = rotatedFireDirection;
                    projectile.tag = Parent.tag;

                    if (StatusEffect != null)
                        projectile.StatusEffects.Add(StatusEffect.Copy());
                }
            }            
        }
    }
}
