using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangeColourAndPlayEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public VisualEffect VFXGraph;
    [SerializeField] public GameObject circleObject;
    [SerializeField] public Material mat;
    void Start()
    {
        this.gameObject.SetActive(false);
        circleObject.SetActive(false);
        VFXGraph.SendEvent("OnHazardStop");
        VFXGraph.SendEvent("OnIceStop");
        VFXGraph.SendEvent("OnRadiationStop");
    }
    public void setColour(Color color)
    {
        mat.color = color;
    }
}
