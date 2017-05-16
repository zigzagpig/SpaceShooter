using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("Game Controller").GetComponent<AudioSource>().Pause();
        Time.timeScale = 0;
	}

    private void OnClick()
    {
        Debug.Log("确实运行到这里了");
        Destroy(GameObject.Find("Simple"));
        Destroy(GameObject.Find("Hard")); 
        Destroy(GameObject.Find("Select"));
        Destroy(GameObject.Find("Note"));
        GameObject.Find("Player").GetComponent<DataManager>().maxHP = 300;
        GameObject.Find("Player").GetComponent<DataManager>().hp = 300;
        GameObject.Find("Game Controller").GetComponent<AudioSource>().Play();
        Time.timeScale = 1;
    }
}
