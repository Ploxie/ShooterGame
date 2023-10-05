using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private GameEvent OnHealthPackPickUpEvent;
    [SerializeField] private GameObject pickUpText;
    [SerializeField] private int healing;

    bool colliding;

    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnHealthPackPickUpEvent.Raise(this, healing);
                gameObject.SetActive(false);
                pickUpText.SetActive(false);
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickUpText.SetActive(true);
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickUpText.SetActive(false);
            colliding = false;
        }
    }
}
