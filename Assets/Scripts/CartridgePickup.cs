using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgePickup : MonoBehaviour
{
    // Start is called before the first frame update

    public ModuleType type;
    public ModuleID id;

    private void Awake()
    {
        //Assign(ModuleType.WeaponModule, id);
    }

    public void Assign(ModuleType type, ModuleID id)
    {
        this.type = type;
        this.id = id;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().PickupModule(type, ModuleRegistry.CreateModuleByID(id));
                Destroy(gameObject);
            }

        }

    }
}
