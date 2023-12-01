using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Entity;

public class PopUpConsumable : MonoBehaviour
{
    public static bool thereCanOnlyBeOne = false;
    private GameObject floatingTextPrefab;
    private GameObject floatingTextInstance;
    private GameObject floatingCharacterPrefab;
    private GameObject floatingCharacterInstance;
    private TMP_Text text;
    private Image image;

    public CartridgePickup CartridgePickup;

    private void Awake()
    {
        //floatingTextInstance = Instantiate(floatingTextPrefab/*, transform.position, Quaternion.identity, transform*/);

        floatingTextPrefab = Resources.Load<GameObject>("Prefabs/UI/CanvasPopUpPrefab");
        floatingTextInstance = Instantiate(floatingTextPrefab, transform);

        floatingCharacterPrefab = Resources.Load<GameObject>("Prefabs/UI/PopUpCharacter");
        floatingCharacterInstance = Instantiate(floatingCharacterPrefab, transform);

        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
        floatingTextInstance.SetActive(false);

        floatingCharacterInstance.SetActive(false);
    }

    private void Start()
    {

        if (CartridgePickup.Module is Weapon)
        {
            image.color = Color.green;
        }
        else if (CartridgePickup.Module is ProjectileEffect)
        {
            image.color = Color.red;
        }
        else if (CartridgePickup.Module is StatusEffect)
        {
            image.color = Color.blue;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !thereCanOnlyBeOne)
        {
            floatingTextInstance.transform.rotation = Camera.main.transform.rotation;
            floatingTextInstance.SetActive(true);
            floatingCharacterInstance.transform.rotation = Camera.main.transform.rotation;
            floatingCharacterInstance.SetActive(true);
            text.text = $"{CartridgePickup.Module.Name} Module";
            thereCanOnlyBeOne = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingTextInstance.SetActive(false);
            floatingCharacterInstance.SetActive(false);
            thereCanOnlyBeOne = false;
        }
    }
}
