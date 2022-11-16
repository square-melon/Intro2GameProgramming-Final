using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseToLastScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleClick()
    {
        DataManager.Instance.SetPlayerHP(200);
        SceneManager.LoadScene(PlayerPrefs.GetInt("SavedScene"));
    }
}
