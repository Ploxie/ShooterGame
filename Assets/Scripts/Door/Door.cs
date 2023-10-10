using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Använda events(??) för att skippa många if satser.

        if (Input.GetKeyDown(KeyCode.P) /*Player.RedKey == true*/) 
        {
            Debug.Log("Open Door");
            anim.SetTrigger("DoorOpen");
        }
        if(Input.GetKeyDown(KeyCode.O) /*Player.RedKey == true*/)
        {
            Debug.Log("Close Door");
            anim.SetTrigger("Reset");
        }
    }
}
