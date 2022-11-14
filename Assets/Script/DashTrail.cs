using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrail : MonoBehaviour
{

    [Header("Settings")]
    public float MeshRefreshRate;
    public Transform PositionToSpawn;
    public float MeshDestroyDelay;

    [Header("Shader")]
    public Material mat;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTrail(float ActivateTime) {
        StartCoroutine(Trail(ActivateTime));
    }

    IEnumerator Trail(float ActivateTime) {
        while (ActivateTime > 0) {
            ActivateTime -= MeshRefreshRate;

            if (skinnedMeshRenderers == null) {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            for (int i = 0; i < skinnedMeshRenderers.Length; i++) {
                GameObject GObj = new GameObject();
                GObj.transform.SetPositionAndRotation(PositionToSpawn.position, PositionToSpawn.rotation);
                MeshRenderer mr = GObj.AddComponent<MeshRenderer>();
                MeshFilter mf =  GObj.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = mat;

                Destroy(GObj, MeshDestroyDelay);
            }

            yield return new WaitForSeconds(MeshRefreshRate);
        }
    }
}
