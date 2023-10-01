using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgePickup : MonoBehaviour
{
    // Start is called before the first frame update

    public Module module;

    public ModuleType type;

    private void Awake()
    {
        Assign(ModuleType.WeaponModule, ModuleGenerator.CreateWeaponModule<ShotgunModule>());
    }

    public void Assign(ModuleType type, Module module)
    {
        this.type = type;
        this.module = module;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.CompareTag("Player"))
            {
                if (module != null && module != null)
                {
                    other.GetComponent<Player>().PickupModule(type, module);
                    Destroy(gameObject);
                }
            }

        }

    }
}
