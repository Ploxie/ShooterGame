using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private LayerMask wallMask;

    [SerializeField][Range(0.0f, 2.0f)] private float cutoutSize;
    [SerializeField][Range(0.0f, 2.0f)] private float falloffSize;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (target == null)
        {
            SetPlayer();
        }
        else
        {
            Vector2 cutoutPosition = mainCamera.WorldToViewportPoint(target.transform.position);
            cutoutPosition.y /= (Screen.width / Screen.height);

            Vector3 offset = target.transform.position - transform.position;
            RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

            for (int i = 0; i < hitObjects.Length; i++)
            {
                Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j].SetVector("_CutoutPosition", cutoutPosition);
                    materials[j].SetFloat("_CutoutSize", cutoutSize / 10);
                    materials[j].SetFloat("_FalloffSize", falloffSize / 10);
                }
            }
        }
        
    }

    private void SetPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
}