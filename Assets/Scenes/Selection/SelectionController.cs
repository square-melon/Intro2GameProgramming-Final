using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public GameObject Maks1;
    public GameObject Mask2;
    public GameObject NoName2;
    public GameObject Name2;
    public GameObject Mask3;
    public GameObject NoName3;
    public GameObject Name3;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.ChapterLevel += 1;
        if(DataManager.Instance.ChapterLevel == 1) {
            Maks1.SetActive(false);
        }
        if(DataManager.Instance.ChapterLevel == 2) {
            Maks1.SetActive(false);
            Mask2.SetActive(false);
            Name2.SetActive(true);
            NoName2.SetActive(false);
        } else if(DataManager.Instance.ChapterLevel == 3) {
            Maks1.SetActive(false);
            Mask2.SetActive(false);
            Name2.SetActive(true);
            NoName2.SetActive(false);
            Mask3.SetActive(false);
            Name3.SetActive(true);
            NoName3.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
