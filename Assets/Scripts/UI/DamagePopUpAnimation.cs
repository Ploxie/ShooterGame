using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;

    private TextMeshProUGUI damageText;
    private float time = 0;
    private Vector3 origin;

    private void Awake()
    {
        damageText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        damageText.color = new Color(1,1,1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
