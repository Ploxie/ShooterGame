﻿using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof(Gun)/*, typeof(GunVisual)*/)]
    public class Player : Character
    {
        public double DashTimeWindow;
        public float DashForce;

        public bool SimulationEnabled;

        //Temporary debug code, dont bother refactoring it
        public TMP_Text WeaponModDebugText;
        public TMP_Text EffectModDebugText;
        public TMP_Text BulletModDebugText;
        //

        public bool inWaveRoom;
        public bool powerUpActive;
        private float powerUpTimer;
        private float fireRateMultiplier;
        private bool fireRateIncreased;
        private int weaponCount;
        private GunVisual gunVisual;
        private Gun Gun { get; set; }
        private PickupAble AvailablePickup { get; set; }
        public Vector3 AimPosition { get; set; }
        public Vector3 moveDirection;
        public Vector3 direction;

        StudioListener sl;

        private readonly ModuleHolder<Weapon> weaponModules = new();
        private readonly ModuleHolder<StatusEffect> effectModules = new();
        private readonly ModuleHolder<ProjectileEffect> bulletModules = new();

        private List<Module> pickedUpModules;

        private bool oneshot = true;

        private Plane plane = new Plane(Vector2.down, 0f);

        public bool IsDashing { get; private set; } = false;

        private float DashDuration = 0.3f;
        private float DashDurationTimer = 0f;

        public float DashCooldown { get; private set; } = 1;
        [field: SerializeField] public float DashCooldownTimer { get; private set; } = 0;
        
        protected override void Awake()
        {
            base.Awake();

            gameObject.tag = "Player";
            Gun = GetComponent<Gun>();
            gunVisual = GetComponentInChildren<GunVisual>();

            if (!SimulationEnabled)
            {
                sl = FindObjectOfType<StudioListener>();
                sl.attenuationObject = this.gameObject;
            }

            weaponModules.Insert(new PistolWeapon());

            pickedUpModules = new List<Module>();

            fireRateIncreased = false;
            powerUpActive = false;

            fireRateMultiplier = 2.0f;
            Health.OnDeath += OnDeath;
            DashCooldownTimer = DashCooldown;
            ////Temporary debug code, dont bother refactoring it
            //WeaponModDebugText = GameObject.Find("weaponModDebugText").GetComponent<TMP_Text>();
            //EffectModDebugText = GameObject.Find("effectModDebugText").GetComponent<TMP_Text>();
            //BulletModDebugText = GameObject.Find("bulletModDebugText").GetComponent<TMP_Text>();
            ////


        }
        protected void Start()
        {
            if (!SimulationEnabled)
            {
                AudioFmodManager.instance.InitializeAmbience(FmodEvents.instance.ambienceTest);
                AudioFmodManager.instance.InitializeMusic(FmodEvents.instance.MusicLoop);
            }

            Health.OnDamageTaken += OnHealthChanged;
            Health.OnHealthGained += OnHealthChanged;

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                Vector3 newPos = new Vector3(transform.position.x, hit.point.y + 1.658768f, transform.position.z);
                transform.transform.position = newPos;
            }
        }
        private void CycleWeapon()
        {
            Gun.ApplyModule(weaponModules.Cycle());
            AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.ToggleWeapon, this.transform.position);
            Weapon temp = weaponModules.Peek();
            if (temp != null)
            {
                WeaponType type = WeaponType.Pistol;
                switch (temp.Name)
                {
                    case "Pistol":
                        type = WeaponType.Pistol;
                        break;
                    case "Sniper":
                        type = WeaponType.BoltAction;
                        break;
                    case "SMG":
                        type = WeaponType.SMG;
                        break;
                    case "Assault Rifle":
                        type = WeaponType.Automatic;
                        break;
                    case "Shotgun":
                        type = WeaponType.Shotgun;
                        break;

                }

                
                //WeaponModDebugText.text = $"Weapon Module: {weaponModules.Peek().Name}";
                gunVisual.UpdateVisuals(type);
                EventManager.GetInstance().TriggerEvent(new PlayerChangeModuleEvent(weaponModules.Peek(), bulletModules.Peek(), effectModules.Peek()));
            }
            //else
            //    WeaponModDebugText.text = "Weapon Module: None";
        }

        private void CycleEffect()
        {
            Gun.ApplyModule(effectModules.Cycle());
            AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.ToggleStatus, this.transform.position);
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
            AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.ToggleProj, this.transform.position);
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

            if (SimulationEnabled)
                return;

            if (oneshot)
            {
                oneshot = false;
                CycleWeapon();
                CycleEffect();
                CycleBullet();
            }

            if (DashCooldownTimer <= DashCooldown)
                DashCooldownTimer += Time.deltaTime;

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

            if (Input.GetKeyDown(KeyCode.LeftShift) && DashCooldownTimer >= DashCooldown)
            {
                DashCooldownTimer = 0;
                IsDashing = true;
            }

            if (!IsDashing)
            {
                float x = Input.GetAxisRaw("Horizontal");
                float z = Input.GetAxisRaw("Vertical");
                moveDirection = Quaternion.Euler(0.0f, Camera.main.transform.localEulerAngles.y, 0.0f) * new Vector3(x, 0.0f, z).normalized;
            }
            else
            {
                DashDurationTimer += Time.deltaTime;
                Rigidbody.AddForce(moveDirection * DashForce * Time.deltaTime, ForceMode.Impulse);
                if (DashDurationTimer >= DashDuration)
                {
                    DashDurationTimer = 0;
                    IsDashing = false;
                }
            }

           

            

            if (Input.GetKeyDown(KeyCode.E)) // All Keycodes should be keybound via unity
            {
                if (!AvailablePickup.IsDestroyed())
                {
                    CartridgePickup cartridgePickup = AvailablePickup as CartridgePickup;
                    if (cartridgePickup != null)
                    {
                        //Gun?.ApplyModule(cartridgePickup.Module);
                        PickupModule(cartridgePickup.Module);
                        EventManager.GetInstance().TriggerEvent(new PlayerPickUpModuleEvent(cartridgePickup.Module));
                        if (!pickedUpModules.Contains(cartridgePickup.Module))
                        {
                            pickedUpModules.Add(cartridgePickup.Module);
                        }
                    }

                    HealthPack healthPack = AvailablePickup as HealthPack;
                    if (healthPack != null)
                    {
                        Health.Heal(healthPack.Healing);
                    }

                    PowerUpPickUp powerUp = AvailablePickup as PowerUpPickUp;
                    if(powerUp != null)
                    {
                        powerUp.Pickup();
                        powerUpActive = true;
                    }

                    AvailablePickup?.Pickup();
                }
                
            }

            weaponCount = weaponModules.GetArray().Count(x => x != null);
            if (powerUpActive)
            {
                powerUpTimer += Time.deltaTime;
                if (!fireRateIncreased)
                {
                    fireRateIncreased = true;
                    for (int i = 0; i < weaponCount; i++)
                    {
                        weaponModules.Cycle().FireRate *= fireRateMultiplier;
                    }
                }

                if (powerUpTimer >= 10)
                {
                    powerUpActive = false;
                    powerUpTimer = 0;
                    fireRateIncreased = false;
                    for (int i = 0; i < weaponCount; i++)
                    {
                        weaponModules.Cycle().FireRate = weaponModules.Peek().DefaultFireRate;
                    }
                }
            }

            

            //Debug.Log(weaponModules.Peek().FireRate);

            //direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //angle -= 90;
            //transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out float distance);
            Vector3 position = ray.GetPoint(distance);
            transform.LookAt(new Vector3(position.x, transform.position.y, position.z));

            direction = position - transform.position;

            Rigidbody.velocity = moveDirection * CurrentMovementSpeed;
        }

        private void PickupModule(Module module)
        {
            if (module is Weapon weapon)
            {
                weaponModules.Insert(weapon);
                AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.PickUpWeapon, this.transform.position);
                CycleWeapon();
                return;
            }

            if (module is StatusEffect statusEffect)
            {
                effectModules.Insert(statusEffect);
                AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.PickUpStatus, this.transform.position);
                CycleEffect();
                return;
            }

            if (module is ProjectileEffect projectileEffect)
            {
                bulletModules.Insert(projectileEffect);
                AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.PickUpProj, this.transform.position);
                CycleBullet();
                return;
            }
        }

        private void OnDeath()
        {
            SceneManager.LoadScene("GameOver");
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

        public ModuleHolder<Weapon> GetWeapons()
        {
            return weaponModules;
        }
        public ModuleHolder<StatusEffect> GetEffects()
        {
            return effectModules;
        }
        public ModuleHolder<ProjectileEffect> GetBullets()
        {
            return bulletModules;
        }

    }

   
}
