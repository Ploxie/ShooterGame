using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class MoreInterestingScrollingIcon : MonoBehaviour
{ 
    private Image image;
    [SerializeField] private GameObject startPosObj;
    private Vector3 startPos;
    [SerializeField] private GameObject endPosObj;
    private Vector3 endPos;
    public int speed;
    ScrollingIconManager manager;


 
    // Start is called before the first frame update
    private void Awake()
    {
        image = GetComponent<Image>();
        startPos = startPosObj.transform.position;
        endPos = endPosObj.transform.position;
    }

    private void Start()
    {
        manager = FindObjectOfType<ScrollingIconManager>();
        manager.SetImage(this);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, endPos) < 10)
        {
            transform.position = startPos;
            manager.SetImage(this);
        }
    }


    // Update is called once per fram

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
