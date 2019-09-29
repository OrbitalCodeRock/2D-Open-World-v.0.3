using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
	public string level;
	public bool additive;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			if (!additive)
				SceneManager.LoadScene (level, LoadSceneMode.Single);
			else
				SceneManager.LoadScene (level, LoadSceneMode.Additive);
		}
	}
}
