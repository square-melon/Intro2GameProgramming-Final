using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour
{

    [Header("References")]
    public GameObject Player;
    public GameObject GameController;
    public GameObject Medkit;
    
    private GameObject PlayerPrefab;
    private GameObject GameControllerPrefab;
    private GameObject MedkitPrefab;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefab = Instantiate(Player);
        GameControllerPrefab = Instantiate(GameController);
        MedkitPrefab = Instantiate(Medkit, new Vector3(-3, 0.5f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
