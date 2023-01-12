using UnityEngine;
using System.Collections;

public class GhostTrailController : MonoBehaviour {

	public GameObject ghostPrefab;
    public float DestroyTime;
    public float Interval;
    public Color color;
    public Material material;
    private bool LCemit;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (DataManager.Instance.EmitGhost) {
            if (LCemit) {
                LCemit = false;
                StartCoroutine(CreateGhost());
            }
        }
	}

    IEnumerator CreateGhost() {
        while (DataManager.Instance.EmitGhost) {
            GameObject ghostObj = Instantiate(ghostPrefab, DataManager.Instance.PlayerPos, DataManager.Instance.PlayerRot);
            ghostObj.transform.localScale = DataManager.Instance.PlayerScale;
            Destroy(ghostObj, DestroyTime);
            SpriteRenderer SR = ghostObj.GetComponent<SpriteRenderer>();
            SR.color = color;
            if (material != null) SR.material = material;
            yield return new WaitForSeconds(Interval);
        }
        LCemit = true;
    }
}