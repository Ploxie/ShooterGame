using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private GameEvent OnHealthPackPickUpEvent;
    [SerializeField] private GameObject pickUpText;
    [SerializeField] private int HealingAmount;

    private bool colliding;

    private void Update()
    {
        if (colliding)
        {
            if (Input.GetKey(KeyCode.E))
            {
                OnHealthPackPickUpEvent.Raise(this, HealingAmount);
                pickUpText.SetActive(false);
                GameObject.Destroy(gameObject);
            }
        }
    }

    // Have to use OnTriggerEnter.
    // OnTriggerStay casuses random lag spikes.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            colliding = true;
            pickUpText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            colliding = false;
            if (pickUpText.active) pickUpText.SetActive(false);
        }
    }
}
