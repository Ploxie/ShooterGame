using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIcon : MonoBehaviour
{
    private Image image;
    public TextMeshProUGUI description;
    public string descriptionText;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        
    }

    private void Update()
    {

    }

    public void SetModule(Module module)
    {
        image.sprite = module.Icon;
        descriptionText = module.Description;
    }

    public void ShowDescription()
    {

        description.text = descriptionText;
        
    }
    public void HideDescription()
    {

        description.text = "";

    }
}
