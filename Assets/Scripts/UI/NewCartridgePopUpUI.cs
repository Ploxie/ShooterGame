using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCartridgePopUpUI : MonoBehaviour
{
    // Start is called before the first frame update

    private Image image;
    private bool triggered = false;
    private double startTime;
    private double timeShow = 5000;
    private void Start()
    {
        image = GetComponentInChildren<Image>();
        image.gameObject.SetActive(false);
        EventManager.GetInstance().AddListener<PlayerPickUpModuleEvent>(TriggerNewPopUp);
    }

    // Update is called once per frame
    private void Update()
    {
        if (triggered)
        {
            if (Utils.GetUnixMillis() - startTime > timeShow)
            {
                triggered = false;
                image.gameObject.SetActive(false);
            }
        }
    }

    private void TriggerNewPopUp(PlayerPickUpModuleEvent e) //ADD: animation + sound
    {
        image.gameObject.SetActive(true);
        image.sprite = e.module.Icon;
        triggered = true;
        startTime = Utils.GetUnixMillis();

    }
}
