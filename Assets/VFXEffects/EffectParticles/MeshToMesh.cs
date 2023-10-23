using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeshToMesh : MonoBehaviour
{
    // Start is called before the first frame update
    private SkinnedMeshRenderer SkinnedMeshRenderer;
    public VisualEffect VFXGraph;
    public float refreshRate;
    void Start()
    {
        SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(UpdateVFXGraph());
    }
    IEnumerator UpdateVFXGraph()
    {
        while (gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            SkinnedMeshRenderer.BakeMesh(m);
            Vector3[] vertices = m.vertices;
            Mesh m2 = new Mesh();
            m2.vertices = vertices;
            VFXGraph.SetMesh("Mesh", m2);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
