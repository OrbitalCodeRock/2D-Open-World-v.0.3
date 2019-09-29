using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float movementSpeed;
	public float speedBonus;

	public Animator animator;
	public GameObject sword; //this is refrencing the empty gameobject that my sword hitbox is attached to
	public GameObject dummy;

	public bool isSprinting;

	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;
	const int STATE_WALKUP = 2;
	const int STATE_WALKDOWN = 3;
	const int STATE_ATTACK = 4;


	public string currentDirection = "left";
	public int currentAnimationState = STATE_IDLE;

	// Use this for initializations
	void Start () {
		isSprinting = false;
		animator = this.GetComponent<Animator> ();

	}
	IEnumerator damagePotion(){
		this.GetComponent<PlayerAttributes> ().att++;
		yield return new WaitForSeconds (10);
		this.GetComponent<PlayerAttributes> ().att--;
	}
	IEnumerator activateSword(){
		yield return new WaitForSeconds (0.5f);
		sword.SetActive (true);
	}
	IEnumerator checkForSwordHit (){ // if the dummy is not currently being hit, deactivate the sword
		yield return new WaitForSeconds (1f);
		if (dummy.GetComponent<Animator> ().GetBool ("wasHit") == false) {
			sword.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		if (this.GetComponent<PlayerAttributes> ().hp <= 0) {
			SceneManager.LoadScene ("DeathScene");
		}
		if (currentAnimationState != STATE_ATTACK) {
			sword.SetActive (false);
		}
		if (this.GetComponent<PlayerAttributes> ().stm <= 0 && isSprinting) {
			movementSpeed -= speedBonus;
			isSprinting = false;
		}
		if (Input.GetKey ("left")) {
			changeDirection ("left");
			if (Input.GetKeyDown (KeyCode.LeftShift) && this.GetComponent<PlayerAttributes>().stm > 0) {
				if(isSprinting == false) isSprinting = true;
				movementSpeed += speedBonus;
			} 
			else if (Input.GetKeyUp (KeyCode.LeftShift) && isSprinting) {
				isSprinting = false;
				movementSpeed -= speedBonus;
			}
			if (Input.GetKey ("a")) {
				changeState (STATE_ATTACK);
				StartCoroutine (activateSword ()); // activating the sword when the attack key is pressed
				if (dummy != null) {
					StartCoroutine (checkForSwordHit ()); // checking if the dummy is currently being hit
				}
			} else if (Input.GetKeyUp ("a")) {
				changeState (STATE_WALK);
				sword.SetActive (false); // if the attack key is let go of, deactivate the sword
			}
			if (currentAnimationState != STATE_ATTACK) {
				transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
				changeState (STATE_WALK);
			}

		} else if (Input.GetKey ("right")) {
			changeDirection ("right");
			if (Input.GetKeyDown (KeyCode.LeftShift) && this.GetComponent<PlayerAttributes>().stm > 0) {
				if(isSprinting == false) isSprinting = true;
				movementSpeed += speedBonus;

			} 
			else if (Input.GetKeyUp (KeyCode.LeftShift) && isSprinting) {
				isSprinting = false;
				movementSpeed -= speedBonus;
			} 	
			if (Input.GetKey ("a")) {
				changeState (STATE_ATTACK);
				StartCoroutine (activateSword ()); // if attack key is pressed while walking right, actiate the sword
				if (dummy != null) {
					StartCoroutine (checkForSwordHit ());
				}
			} else if (Input.GetKeyUp ("a")) {
				changeState (STATE_WALK);
				sword.SetActive (false); // if the attack key is let go of, deactivate the sword
			}
			if (currentAnimationState != STATE_ATTACK) {
				transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
				changeState (STATE_WALK);
			}
		} else if (Input.GetKey ("down")) {
			changeState (STATE_WALKDOWN);
			if (Input.GetKeyDown (KeyCode.LeftShift) && this.GetComponent<PlayerAttributes>().stm > 0) {
				if(isSprinting == false) isSprinting = true;
				movementSpeed += speedBonus;

			} 
			else if (Input.GetKeyUp (KeyCode.LeftShift) && isSprinting) {
				isSprinting = false;
				movementSpeed -= speedBonus;
			}
			transform.Translate (Vector3.down * movementSpeed * Time.deltaTime);
		} else if (Input.GetKey ("up")) {
			changeState (STATE_WALKUP);
			if (Input.GetKeyDown (KeyCode.LeftShift) && this.GetComponent<PlayerAttributes>().stm > 0) {
				if(isSprinting == false) isSprinting = true;
				movementSpeed += speedBonus;

			} 
			else if (Input.GetKeyUp (KeyCode.LeftShift) && isSprinting) {
				isSprinting = false;
				movementSpeed -= speedBonus;
			}
			transform.Translate (Vector3.up * movementSpeed * Time.deltaTime);
		} else if (Input.GetKey ("a")) {
			changeState (STATE_ATTACK);
			StartCoroutine (activateSword ()); // activating the sword when the attack key is pressed
			if (dummy != null) { // checking if the dummy is currently being hit
				StartCoroutine (checkForSwordHit ());
			}

		} else if (Input.GetKeyUp ("a")) {
			sword.SetActive (false); // if attack key is let go of, deativate the sword
		}
		else if (Input.GetKeyDown (KeyCode.LeftShift) && this.GetComponent<PlayerAttributes>().stm > 0) {
			if(isSprinting == false) isSprinting = true;
			movementSpeed += speedBonus;

		} 
		else if (Input.GetKeyUp (KeyCode.LeftShift) && isSprinting) {
			isSprinting = false;
			movementSpeed -= speedBonus;
		}


		else {
		changeState (STATE_IDLE);

		}

	}
	void changeDirection(string direction)
	{

		if (currentDirection != direction)
		{
			if (direction == "right")
			{
				transform.Rotate (0, 180, 0);
				currentDirection = "right";
			}
			else if (direction == "left")
			{
				transform.Rotate (0, -180, 0);
				currentDirection = "left";
			}
		}
	}
	void changeState(int state){

		if (currentAnimationState == state)
			return;

		switch (state) {

		case STATE_WALK:
			animator.SetInteger ("state", STATE_WALK);
			break;
	
		case STATE_IDLE:
			animator.SetInteger ("state", STATE_IDLE);
			break;

		case STATE_WALKUP:
			animator.SetInteger ("state", STATE_WALKUP);
			break;

		case STATE_WALKDOWN:
			animator.SetInteger ("state", STATE_WALKDOWN);
			break;

		case STATE_ATTACK:
			animator.SetInteger ("state", STATE_ATTACK);
			break;				
		}

		currentAnimationState = state;
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "damagePotion") {
			Destroy (col.gameObject);
			StartCoroutine (damagePotion ());
		}
		else if (col.gameObject.tag == "healthPotion") {
			Destroy (col.gameObject);
			this.GetComponent<PlayerAttributes> ().hp += this.GetComponent<PlayerAttributes>().maxHp/10;
		}
	}
}
