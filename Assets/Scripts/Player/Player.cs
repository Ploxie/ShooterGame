using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof(Gun)/*, typeof(GunVisual)*/)]
    public class Player : Character
    {
        public double DashTimeWindow;
        public float DashForce;

        //Temporary debug code, dont bother refactoring it
        public TMP_Text WeaponModDebugText;
        public TMP_Text EffectModDebugText;
        public TMP_Text BulletModDebugText;
        //

        public bool inWaveRoom;
        private Gun Gun { get; set; }
        private PickupAble AvailablePickup { get; set; }
        public Vector3 AimPosition { get; set; }
        public Vector3 moveDirection;
        public Vector3 direction;

        private readonly ModuleHolder<Weapon> weaponModules = new();
        private readonly ModuleHolder<StatusEffect> effectModules = new();
        private readonly ModuleHolder<ProjectileEffect> bulletModules = new();

        protected override void Awake()
        {
            base.Awake();

            gameObject.tag = "Player";
            Gun = GetComponent<Gun>();

            weaponModules.Insert(new ShotgunWeapon());

            ////Temporary debug code, dont bother refactoring it
            //WeaponModDebugText = GameObject.Find("weaponModDebugText").GetComponent<TMP_Text>();
            //EffectModDebugText = GameObject.Find("effectModDebugText").GetComponent<TMP_Text>();
            //BulletModDebugText = GameObject.Find("bulletModDebugText").GetComponent<TMP_Text>();
            ////

            CycleWeapon();
            CycleEffect();
            CycleBullet();

            Health.OnDamageTaken += OnHealthChanged;
            Health.OnHealthGained += OnHealthChanged;
        }
        
        private void CycleWeapon()
        {
            Gun.ApplyModule(weaponModules.Cycle());
            if (weaponModules.Peek() != null)
            {
                //WeaponModDebugText.text = $"Weapon Module: {weaponModules.Peek().Name}";
                EventManager.GetInstance().TriggerEvent(new PlayerChangeModuleEvent(weaponModules.Peek(), bulletModules.Peek(), effectModules.Peek()));
            }
            //else
            //    WeaponModDebugText.text = "Weapon Module: None";
        }

        private void CycleEffect()
        {
            Gun.ApplyModule(effectModules.Cycle());
            if (effectModules.Peek() != null)
            {
                //EffectModDebugText.text = $"Effect Module: {effectModules.Peek().Name}";
                EventManager.GetInstance().TriggerEvent(new PlayerChangeModuleEvent(weaponModules.Peek(), bulletModules.Peek(), effectModules.Peek()));
            }
            //else
            //    EffectModDebugText.text = "Effect Module: None";
        }

        private void CycleBullet()
        {
            Gun.ApplyModule(bulletModules.Cycle());
            if (bulletModules.Peek() != null)
            {
                //BulletModDebugText.text = $"Bullet Module: {bulletModules.Peek().Name}";
                EventManager.GetInstance().TriggerEvent(new PlayerChangeModuleEvent(weaponModules.Peek(), bulletModules.Peek(), effectModules.Peek()));
            }
            //else
            //    BulletModDebugText.text = "Bullet Module: None";
        }
        
        protected override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Gun?.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
                CycleWeapon();

            if (Input.GetKeyDown(KeyCode.Alpha2))
                CycleEffect();

            if (Input.GetKeyDown(KeyCode.Alpha3))
                CycleBullet();
                

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            moveDirection = Quaternion.Euler(0.0f, Camera.main.transform.localEulerAngles.y, 0.0f) * new Vector3(x, 0.0f, z).normalized;

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

            direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
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
                CycleWeapon();
                return;
            }

            if (module is StatusEffect statusEffect)
            {
                effectModules.Insert(statusEffect);
                CycleEffect();
                return;
            }

            if (module is ProjectileEffect projectileEffect)
            {
                bulletModules.Insert(projectileEffect);
                CycleBullet();
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
