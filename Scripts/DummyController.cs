using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour { //this is a training dummy that the player will be hitting
	public GameObject player;
	public GameObject sword;

	public int maxHp;
	public int hp;
	public int def;

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
		
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerController>().animator.GetInteger("state") != 4){
			animator.SetBool ("wasHit", false);
		}
		if (hp <= 0) {
			animator.SetBool ("wasHit", false);
			Destroy (this.GetComponent<BoxCollider2D> ());
			animator.SetBool ("isDead", true);
			Invoke ("Death", 2f);
		}
	}		
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == ("Sword")) {
			if (player.GetComponent<PlayerController>().animator.GetInteger("state") == 4){
				animator.SetBool ("wasHit", true);
				hp = hp + def - player.GetComponent<PlayerAttributes>().att;
				sword.SetActive (false); // if the player hits the dummy, deactivate the sword

			}
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == ("Sword")) {
			sword.SetActive (false); // on the exit of the collision, deactivate the sword
		}


	}
	void Death (){
		player.GetComponent<PlayerController> ().dummy = null;
		Destroy (this.gameObject);
	}
}
