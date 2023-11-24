using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : PickupAble
{
    // Start is called before the first frame update

    private GameObject floatingCharacterPrefab;
    private GameObject floatingCharacterInstance;

    private void Awake()
    {
        floatingCharacterPrefab = Resources.Load<GameObject>("Prefabs/UI/PopUpCharacter");
        floatingCharacterInstance = Instantiate(floatingCharacterPrefab, transform);
    }
    public override void Pickup()
    {
        base.Pickup();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingCharacterInstance.transform.rotation = Camera.main.transform.rotation;
            floatingCharacterInstance.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingCharacterInstance.SetActive(false);
        }
    }
}
