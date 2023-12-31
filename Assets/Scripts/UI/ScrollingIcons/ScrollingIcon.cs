using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScrollingIcon : MonoBehaviour
{
    private Image image;
    public ScrollingIconManager Manager;
    // Start is called before the first frame update
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
