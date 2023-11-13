using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask wallMask;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPosition = mainCamera.WorldToViewportPoint(target.position);
        cutoutPosition.y /= (Screen.width / Screen.height);

        Vector3 offset = target.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for(int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for(int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutoutPosition", cutoutPosition);
                materials[j].SetFloat("_CutoutSize", 0.1f);
                materials[j].SetFloat("_FalloffSize", 0.05f);
            }
        }
    }
}
