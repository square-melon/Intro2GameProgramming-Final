using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;


//using static ScoreManager;
public class ControlScore : MonoBehaviour
{
    // Start is called before the first frame update
    //private float score;
    
    
    public Animator animator;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        int xCenter = (Screen.width / 2);
        int yCenter = (Screen.height / 2);
        int width = 400;
        int height = 120;

        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("button"));
        fontSize.fontSize = 32;

        Scene scene = SceneManager.GetActiveScene();
        
        if (scene.name == "Scenetest")
        {
            //Show a button to allow scene2 to be switched to.
            if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Load second scene", fontSize))
            {
                // ScoreManager.Instance.IncreaseScore(25.0f); 
                // SceneManager.LoadScene("Scene2");
                //animator.SetTrigger("FadeOut");
            }
        }
        else if(scene.name == "Scene2")
        {
            // Show a button to allow scene1 to be returned to.
            // if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Return to first scene", fontSize))
            // {
            //     // ScoreManager.Instance.IncreaseScore(25.0f); 
            //     animator.SetTrigger("FadeOut");
                
                
            // }
        }
        
    }
    public void LoadtoNextScene() {
        SceneManager.LoadScene("Scenetest");
    }
}