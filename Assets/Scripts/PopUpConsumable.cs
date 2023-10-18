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

    private CartridgePickup cartridgePickup;

    private void Start()
    {
        //floatingTextInstance = Instantiate(floatingTextPrefab/*, transform.position, Quaternion.identity, transform*/);
        cartridgePickup = GetComponent<CartridgePickup>();
        floatingTextInstance = Instantiate(floatingTextPrefab, transform);
        text = GetComponentInChildren<TextMeshProUGUI>();        
        floatingTextInstance.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            floatingTextInstance.SetActive(true);
            text.text = $"{cartridgePickup.Module.Name} Module";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            floatingTextInstance.SetActive(false);
        }
    }
}
