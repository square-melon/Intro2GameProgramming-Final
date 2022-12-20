using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PanAndZoom : MonoBehaviour
{

    [Header("References")]

    [Header("Settings")]
    public float CameraZAxisCtrl;
    public float CameraYAxisCrtl;
    public float PanSpeed;
    public float BorderUp;
    public float BorderDown;
    public float BorderLeft;
    public float BorderRight;
    
    private CinemachineVirtualCamera VirtualCamera;
    private Transform CameraTransform;
    private GameController gam;
    private bool Switching;
    private bool Lock;
    private bool first;

    void Start()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CameraTransform = VirtualCamera.VirtualCameraGameObject.transform;
        Init();
        first = false;
    }

    void Init() {
        Center();
        StartCoroutine(CenterAfterHalfSec());
        Switching = false;
        Lock = false;
    }

    void Update()
    {
        if (first == false) {
            Center();
            first = true;
        }
        if (DataManager.Instance.IsPlayerDead) {
            Center();
        } else if (DataManager.Instance.InParallel != Switching) {
            StartCoroutine(CenterAfterHalfSec());
        } else {
            if (Input.GetKeyDown(KeyCode.Y))
                Lock = !Lock;
            MoveCamera();
        }
        Switching = DataManager.Instance.InParallel;
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

    IEnumerator CenterAfterHalfSec() {
        yield return new WaitForSeconds(0.5f);
        Center();
    }

    void Center() {
        Vector3 playerPos = DataManager.Instance.PlayerPos;
        CameraTransform.position = new Vector3(playerPos.x, playerPos.y + CameraYAxisCrtl, playerPos.z - CameraZAxisCtrl);
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
