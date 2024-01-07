using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGlow : MonoBehaviour
{
    private Material material;
    [SerializeField] private AnimationCurve curve;
    private float timePassed = 0;
    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        material.SetFloat("_EmissiveExposureWeight", curve.Evaluate(timePassed % 5));
    }
}
