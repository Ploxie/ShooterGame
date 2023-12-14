using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangeColourAndPlayEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public VisualEffect VFXGraph;
    [SerializeField] public GameObject circleObject;
    public Material mat;
    void Start()
    {
        this.gameObject.SetActive(false);
        circleObject.SetActive(false);
        VFXGraph.SendEvent("OnHazardStop");
        VFXGraph.SendEvent("OnIceStop");
        VFXGraph.SendEvent("OnRadiationStop");
        mat = GetComponent<Renderer>().material;
    }
    public void setColour(Color color)
    {
        mat.SetColor("_EmissionColor",color);
    }
}
