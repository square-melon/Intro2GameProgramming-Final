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
    
    public Text m_Text;
    public Animator animator;
    // Update is called once per frame
    void Update()
    {
        
    }
    // private void OnGUI()
    // {
    //     int xCenter = (Screen.width / 2);
    //     int yCenter = (Screen.height / 2);
    //     int width = 400;
    //     int height = 120;
    //     
    //     GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("button"));
    //     fontSize.fontSize = 32;

    //     Scene scene = SceneManager.GetActiveScene();
        
    //     if (scene.name == "Scenetest")
    //     {
    //         //Show a button to allow scene2 to be switched to.
    //         if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Load second scene", fontSize))
    //         {
    //             // ScoreManager.Instance.IncreaseScore(25.0f); 
    //             // SceneManager.LoadScene("Scene2");
    //             //animator.SetTrigger("FadeOut");
    //         }
    //     }
    //     else if(scene.name == "Scene2")
    //     {
    //         // Show a button to allow scene1 to be returned to.
    //         // if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Return to first scene", fontSize))
    //         // {
    //         //     // ScoreManager.Instance.IncreaseScore(25.0f); 
    //         //     animator.SetTrigger("FadeOut");
                
                
    //         // }
    //     }
    //     else if(scene.name == "Menu") {
    //         //animator.SetTrigger("FadeOut");
    //     }
        
    // }
    public void LoadtoNextScene() {
        StartCoroutine(Load());
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator Load() {
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        // while (!asyncLoad.isDone)
        // {
        //     yield return null;
        // }
         yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}