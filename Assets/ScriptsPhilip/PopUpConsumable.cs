using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpConsumable : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    private GameObject floatingTextInstance;

    private void Start()
    {
        floatingTextInstance = Instantiate(floatingTextPrefab);
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
