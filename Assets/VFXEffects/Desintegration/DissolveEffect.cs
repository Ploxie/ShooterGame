using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveEffect : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public VisualEffect Effect;

    private Material[] materials;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    // Start is called before the first frame update
    void Start()
    {
        if (SkinnedMeshRenderer != null)
        {
            materials = SkinnedMeshRenderer.materials;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            StartCoroutine(DissolveRoutine());
            
        }
    }
    public IEnumerator DissolveRoutine()
    {
        if (materials.Length >0)
        {
            Effect.Play();
            float counter = 0;
            while (materials[0].GetFloat("_DissolveAmount") < 1)
            {
                //decrease
                counter += dissolveRate;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
