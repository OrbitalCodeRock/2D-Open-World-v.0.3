using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {
	public GameObject thing;
	public float scale;
	// Use this for initializations
	void Start () {
	}

	// Update is called once per frame
	void Update ()
	{
		if (thing.tag == "Player") {
			this.GetComponent<Transform> ().localScale = new Vector3 (thing.GetComponent<PlayerAttributes> ().hp * scale, this.GetComponent<Transform> ().localScale.y);
			if (thing.GetComponent<PlayerAttributes> ().hp == 0) {
				Destroy (this);
			} 
		} else if (thing.tag == "dummy") {
			this.GetComponent<Transform> ().localScale = new Vector3 (thing.GetComponent<DummyController> ().hp * scale, this.GetComponent<Transform> ().localScale.y);
			if (thing.GetComponent<DummyController> ().hp == 0) {
				Destroy (this);
			}
		} 
		else if (thing.tag == "skeleton") {
			this.GetComponent<Transform> ().localScale = new Vector3 (thing.GetComponent<SkeletonAttributes> ().hp * scale, this.GetComponent<Transform> ().localScale.y);
			if (thing.GetComponent<SkeletonAttributes> ().hp == 0) {
				Destroy (this);
			}
		}

	}
}

