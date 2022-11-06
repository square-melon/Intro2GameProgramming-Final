using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour
{

    [Header("References")]
    public GameObject Player;
    public GameObject GameController;
    
    private GameObject PlayerPrefab;
    private GameObject GameControllerPrefab;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefab = Instantiate(Player);
        GameControllerPrefab = Instantiate(GameController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
