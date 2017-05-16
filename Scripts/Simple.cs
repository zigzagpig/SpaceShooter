using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnClick()
    {
        Debug.Log("确实运行到这里了");
        Destroy(GameObject.Find("Simple"));
        Destroy(GameObject.Find("Hard"));
        Destroy(GameObject.Find("Select"));
        Destroy(GameObject.Find("Note"));
        GameObject.Find("Player").GetComponent<DataManager>().maxHP = 1000;
        GameObject.Find("Player").GetComponent<DataManager>().hp = 1000;
        GameObject.Find("Game Controller").GetComponent<AudioSource>().Play();
        Time.timeScale = 1;
    }
}
