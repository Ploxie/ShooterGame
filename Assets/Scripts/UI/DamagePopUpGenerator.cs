using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator Instance;
    public GameObject prefab;
    public Transform target;

    private void Awake()
    {
        Instance = this;
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            CreatePopUp(new Vector3(0, 4), Random.Range(0, 1000).ToString());
        }
        prefab.transform.forward = target.forward;
    }

    public void CreatePopUp(Vector3 position, string text)
    {
        var popup = Instantiate(prefab, position, Quaternion.LookRotation(prefab.transform.forward));
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popup, 1f);
    }
}
