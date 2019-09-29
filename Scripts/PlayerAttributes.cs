using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {
	public int maxHp;
	public int hp;
	public int att;
	public int def;
	public int maxStm;
	public int stm;
	// Use this for initialization
	IEnumerator playWoosh(){
		yield return new WaitForSeconds (0.1f);
		this.GetComponent<AudioSource> ().Play();
	}
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(this.GetComponent<PlayerController> ().currentAnimationState == 4 && !this.GetComponent<AudioSource>().isPlaying) {
			StartCoroutine (playWoosh ());
		}
	}
}
