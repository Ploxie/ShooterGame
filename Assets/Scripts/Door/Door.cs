using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    private bool isOpen;
    private bool openDoor;
    private bool closeDoor;
    private bool inTrigger;
    public GameObject imageE;

    void Awake()
    {
        anim = GetComponent<Animator>();
        isOpen = false;
        openDoor = false;
        closeDoor = false;
        imageE.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Använda events(??) för att skippa många if satser.

        if (Input.GetKeyDown(KeyCode.E) && isOpen == false && inTrigger == true /* && Player.RedKey == true*/)
        {
            Debug.Log("Open Door");
            openDoor = true;
            closeDoor = false;
            //anim.SetTrigger("DoorOpen");
        }
        if (Input.GetKeyDown(KeyCode.E) && isOpen == true && inTrigger == true /* && Player.RedKey == true*/)
        {
            Debug.Log("Close Door");
            closeDoor = true;
            openDoor = false;
            //anim.SetTrigger("Reset");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        imageE.SetActive(true);
        inTrigger = true;

    }
    private void OnTriggerStay(Collider other)
    {
        if (isOpen == false)
        {
            if (openDoor == true)
            {
                Debug.Log("Open Door");
                isOpen = true;
                anim.SetTrigger("DoorOpen");
            }
        }
        if (isOpen == true)
        {
            if (openDoor == false)
            {
                Debug.Log("Close Door");
                isOpen = false;
                anim.SetTrigger("Reset");
            }
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        Debug.Log("Exited");
        imageE.SetActive(false);
        inTrigger = false;
    }
}