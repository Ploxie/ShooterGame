﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof(Gun)/*, typeof(GunVisual)*/)]
    public class Player : Character
    {
        public double DashTimeWindow;
        public float DashForce;

        private Gun Gun { get; set; }
        private PickupAble AvailablePickup { get; set; }
        public Vector3 AimPosition { get; set; }

        private ModuleHolder<Weapon> weaponModules = new();
        private ModuleHolder<StatusEffect> effectModules = new();
        private ModuleHolder<ProjectileEffect> bulletModules = new();

        protected override void Awake()
        {
            base.Awake();

            gameObject.tag = "Player";
            Gun = GetComponent<Gun>();

            weaponModules.Insert(new ShotgunWeapon());

            Gun.ApplyModule(weaponModules.Peek());
            Gun.ApplyModule(effectModules.Peek());
            Gun.ApplyModule(bulletModules.Peek());

            Health.OnDamageTaken += OnHealthChanged;
            Health.OnHealthGained += OnHealthChanged;
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Gun?.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
                Gun.ApplyModule(weaponModules.Cycle());

            if (Input.GetKeyDown(KeyCode.Alpha2))
                Gun.ApplyModule(effectModules.Cycle());

            if (Input.GetKeyDown(KeyCode.Alpha3))
                Gun.ApplyModule(bulletModules.Cycle());

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 moveDirection = Quaternion.Euler(0.0f, Camera.main.transform.localEulerAngles.y, 0.0f) * new Vector3(x, 0.0f, z).normalized;

            if (Input.GetKeyDown(KeyCode.Space))
                Rigidbody.AddForce(moveDirection * DashForce);

            if (Input.GetKeyDown(KeyCode.E)) // All Keycodes should be keybound via unity
            {
                if (!AvailablePickup.IsDestroyed())
                {
                    CartridgePickup cartridgePickup = AvailablePickup as CartridgePickup;
                    if (cartridgePickup != null)
                    {
                        //Gun?.ApplyModule(cartridgePickup.Module);
                        PickupModule(cartridgePickup.Module);
                    }

                    HealthPack healthPack = AvailablePickup as HealthPack;
                    if (healthPack != null)
                    {
                        Health.Heal(healthPack.Healing);
                    }

                    AvailablePickup?.Pickup();
                }
            }

            Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
            
            Rigidbody.velocity = moveDirection * CurrentMovementSpeed;
        }

        private void PickupModule(Module module)
        {
            if (module is Weapon weapon)
            {
                weaponModules.Insert(weapon);
                return;
            }

            if (module is StatusEffect statusEffect)
            {
                effectModules.Insert(statusEffect);
                return;
            }

            if (module is ProjectileEffect projectileEffect)
            {
                bulletModules.Insert(projectileEffect);
                return;
            }
        }

        private void OnHealthChanged(float _)
        {
            EventManager.GetInstance().TriggerEvent(new PlayerHealthChangeEvent(Health));
        }

        protected void OnTriggerEnter(Collider other)
        {            
            if(other.TryGetComponent(out PickupAble pickup))
            {
                AvailablePickup = pickup;
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PickupAble pickup))
            {
                AvailablePickup = null;
                return;
            }
        }

    }
}
