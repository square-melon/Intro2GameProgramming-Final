using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PanAndZoom : MonoBehaviour
{

    [Header("References")]
    public GameObject gameController;

    [Header("Settings")]
    public float CameraZAxisCtrl;
    public float PanSpeed;
    public float BorderUp;
    public float BorderDown;
    public float BorderLeft;
    public float BorderRight;
    
    private CinemachineVirtualCamera VirtualCamera;
    private Transform CameraTransform;
    private GameController gam;
    private bool Lock;

    void Start()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        gam = gameController.GetComponent<GameController>();
        CameraTransform = VirtualCamera.VirtualCameraGameObject.transform;
        Init();
    }

    void Init() {
        Center();
        Lock = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            Lock = !Lock;
        MoveCamera();
    }

    void MoveCamera() {
        if (Lock) {
            Center();
        } else {
            if (Input.GetKey(KeyCode.Space)) {
                Center();
            } else {
                PanScreen();
            }
        }
    }

    void Center() {
        Vector3 playerPos = gam.PlayerPos();
        CameraTransform.position = new Vector3(playerPos.x, CameraTransform.position.y, playerPos.z - CameraZAxisCtrl);
    }

    void PanScreen() {
        Vector3 dir = PanDir();
        CameraTransform.position = Vector3.Lerp(CameraTransform.position, CameraTransform.position + dir * PanSpeed, Time.deltaTime);
    }

    Vector3 PanDir() {
        Vector3 direction = Vector3.zero;
        if (Input.mousePosition.y >= Screen.height * BorderUp)
            direction.z += 1;
        else if (Input.mousePosition.y <= Screen.height * BorderDown)
            direction.z -= 1;
        if (Input.mousePosition.x >= Screen.width * BorderRight)
            direction.x += 1;
        else if (Input.mousePosition.x <= Screen.width * BorderLeft)
            direction.x -= 1;
        return direction;
    }
    
}
