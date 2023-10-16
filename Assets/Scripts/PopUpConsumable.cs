using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpConsumable : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    private GameObject floatingTextInstance;
    private TMP_Text text;

    CartridgePickup cartridgePickup;

    private void Start()
    {
        //floatingTextInstance = Instantiate(floatingTextPrefab/*, transform.position, Quaternion.identity, transform*/);
        cartridgePickup = GetComponentInParent<CartridgePickup>();
        floatingTextInstance = Instantiate(floatingTextPrefab, transform);
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{cartridgePickup.Module.Name} Module";
        floatingTextInstance.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            floatingTextInstance.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            floatingTextInstance.SetActive(false);
        }
    }
}
