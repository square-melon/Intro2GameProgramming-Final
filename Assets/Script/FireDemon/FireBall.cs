using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float ExistTime;
    public GameObject FirePlace;
    public Vector3 AdjustInsPlace;
    public float ExplodeDis;

    void Start()
    {
        Destroy(gameObject, ExistTime);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit[] hit = Physics.RaycastAll(ray, 0.5f);
        foreach (var obj in hit) {
            if (obj.collider.CompareTag("Ground")) {
                Vector3 P = transform.position;
                Instantiate(FirePlace, P + AdjustInsPlace, Quaternion.identity);
                P.y = 0;
                Vector3 X = DataManager.Instance.PlayerPos;
                X.y = 0;
                float d = Vector3.Distance(P, X);
                if (d <= ExplodeDis)
                    DataManager.Instance.BurnPlayer();
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("Ground")) {
            Vector3 P = transform.position;
            Instantiate(FirePlace, P + AdjustInsPlace, Quaternion.identity);
            float d = Vector3.Distance(DataManager.Instance.PlayerPos, transform.position);
            if (d <= ExplodeDis)
                DataManager.Instance.BurnPlayer();
            Destroy(gameObject);
        }
    }
}
