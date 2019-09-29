using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class buttonLevelLoader : MonoBehaviour {
	public string level;

	Button btn;
	void Start () {
		btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void TaskOnClick(){
		SceneManager.LoadScene (level);
	}
}
