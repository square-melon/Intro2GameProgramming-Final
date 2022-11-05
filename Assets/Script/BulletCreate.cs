using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreate : MonoBehaviour
{

    [Header("Settings")]
    public float ExistTime;
    public GameObject GameControllerObj;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, ExistTime);
        gameController = GameControllerObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectShootOn();
    }

    void DetectShootOn() {
        Ray ray = new Ray(transform.position, GetComponent<Rigidbody>().velocity);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f)) {
            if (hit.collider.CompareTag("Enemy")) {
                gameController.PlayerBulletHitOn(hit.collider.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (!other.collider.CompareTag("Player"))
            Destroy(gameObject);
    }
}
