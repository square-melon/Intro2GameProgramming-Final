using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitAnimation : MonoBehaviour
{
    [Header("References")]
    public GameObject gameController;

    [Header("Settings")]
    public float RotationSpeed;
    public float MovingSpeed;
    public float UpLimit;
    public float DownLimit;

    private GameController gam;
    private bool MovingDir;

    // Start is called before the first frame update
    void Start()
    {
        gam = gameController.GetComponent<GameController>();
        gameObject.SetActive(true);
        MovingDir = true;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        MovingUpAndDown();
    }

    void Rotation() {
        transform.eulerAngles += new Vector3(0, RotationSpeed, 0);
    }

    void MovingUpAndDown() {
        if (transform.position.y >= UpLimit)
            MovingDir = false;
        else if (transform.position.y <= DownLimit)
            MovingDir = true;
        
        if (MovingDir)
            transform.position += new Vector3(0, MovingSpeed, 0);
        else
            transform.position -= new Vector3(0, MovingSpeed, 0);
    }
}
