using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {
	public GameObject target;
	public GameObject sword;
	public GameObject gameManager;
	public Transform hpPot;
	public Transform dmgPot;

	public int movementSpeed;
	public float attackRange;

	private bool isDead;

	private bool check;

	float targetX;
	float targetY;
	float skeletonX;
	float skeletonY;


	const int STATE_IDLE = 0;
	const int STATE_WALK = 1;
	const int STATE_WALKUP = 2;
	const int STATE_WALKDOWN = 3;
	const int STATE_ATTACK = 4;
	const int STATE_DEATH = 5;

	public string currentDirection = "right";
	public int currentAnimationState = STATE_IDLE;

	public bool targetInRange;

	Animator animator;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		isDead = false;
		check = true;
	}
	IEnumerator attackTarget(){
		yield return new WaitForSeconds (1f);
		if (currentAnimationState != STATE_ATTACK) yield break;
		target.GetComponent<PlayerAttributes> ().hp -= this.GetComponent<SkeletonAttributes> ().att;

	}
	// Update is called once per frame
	void Update () {
		targetX = target.GetComponent<Transform> ().position.x;
		targetY = target.GetComponent<Transform> ().position.y;
		skeletonX = this.GetComponent<Transform> ().position.x;
		skeletonY = this.GetComponent<Transform> ().position.y;

		if (this.GetComponent<SkeletonAttributes> ().hp <= 0) {
			isDead = true;
			Destroy (this.GetComponent<BoxCollider2D> ());
			changeState (STATE_DEATH);
			Invoke ("Death", 3f);
		}
		if (targetX > skeletonX && !targetInRange && currentAnimationState != STATE_DEATH) {
			changeDirection ("left");
			if (targetY > skeletonY) {
				if (targetY - skeletonY < 3) {
					changeState (STATE_WALK);
				} else {
					changeState (STATE_WALKUP);
				}
				transform.Translate (new Vector3 (-1 * movementSpeed * Time.deltaTime, 1 * movementSpeed * Time.deltaTime, 0));
			} else if (targetY < skeletonY) {	
				if (skeletonY - targetY < 3) {
					changeState (STATE_WALK);
				} else {
					changeState (STATE_WALKUP);
				}
				changeState (STATE_WALKDOWN);
				transform.Translate (new Vector3 (-1 * movementSpeed * Time.deltaTime, -1 * movementSpeed * Time.deltaTime, 0));
			} else {
				changeState (STATE_WALK);
				transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
			}
				
		} else if (targetX < skeletonX && !targetInRange && currentAnimationState != STATE_DEATH) {
			changeDirection ("right");
			if (targetY > skeletonY) {
				if (targetY - skeletonY < 3) {
					changeState (STATE_WALK);
				} else {
					changeState (STATE_WALKUP);
				}
				transform.Translate (new Vector3 (-1 * movementSpeed * Time.deltaTime, 1 * movementSpeed * Time.deltaTime, 0));
			} else if (targetY < skeletonY) {
				if (skeletonY - targetY < 3) {
					changeState (STATE_WALK);
				} else {
					changeState (STATE_WALKDOWN);
				}
				transform.Translate (new Vector3 (-1 * movementSpeed * Time.deltaTime, -1 * movementSpeed * Time.deltaTime, 0));
			} else {
				changeState (STATE_WALK);
				transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
			}	



		}
		else if (targetInRange && currentAnimationState != STATE_DEATH) {
			changeState (STATE_ATTACK);
		}
			
 }
	void FixedUpdate(){
		Transform skeleton = this.GetComponent<Transform> ();

		RaycastHit2D hitLeft = Physics2D.Raycast (new Vector3(skeleton.position.x + 3, skeleton.position.y, skeleton.position.z), Vector2.left, attackRange);
		RaycastHit2D hitRight = Physics2D.Raycast (new Vector3(skeleton.position.x - 3, skeleton.position.y, skeleton.position.z), Vector2.right, attackRange);
		RaycastHit2D hitUp = Physics2D.Raycast (new Vector3(skeleton.position.x, skeleton.position.y - 4, skeleton.position.z), Vector2.up, attackRange);
		RaycastHit2D hitDown = Physics2D.Raycast (new Vector3(skeleton.position.x, skeleton.position.y + 4, skeleton.position.z), Vector2.down, attackRange);

		if (isDead == false) {
			if (hitLeft.transform.gameObject.tag != null || hitRight.transform.gameObject.tag != null || hitUp.transform.gameObject.tag != null || hitDown.transform.gameObject.tag != null && currentAnimationState != STATE_DEATH) {
				if (hitLeft.transform.gameObject.tag == "Player" || hitRight.transform.gameObject.tag == "Player") {
					targetInRange = true;
					StartCoroutine (attackTarget ());
				} else {
					targetInRange = false;
				}
			}
		} else {
			changeState (STATE_DEATH);
		}
}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == ("Sword")) {
			if (target.GetComponent<PlayerController>().animator.GetInteger("state") == 4){
				this.GetComponent<SkeletonAttributes>().hp = this.GetComponent<SkeletonAttributes>().hp + this.GetComponent<SkeletonAttributes>().def - target.GetComponent<PlayerAttributes>().att;
				sword.SetActive (false); // if the player hits the dummy, deactivate the sword

			}
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == ("Sword")) {
			sword.SetActive (false); // on the exit of the collision, deactivate the sword
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
	void Death(){
		if (check) {
			gameManager.GetComponent<WaveScript> ().enemiesLeft--;
			check = false;
		}
		int rng = Random.Range (1, 4);
		if (rng == 1) {
			Instantiate (hpPot, new Vector3(skeletonX, skeletonY), Quaternion.identity);
		}
		else if (rng == 2) {
			Instantiate (dmgPot, new Vector3(skeletonX, skeletonY), Quaternion.identity);
		}
		Destroy (this.gameObject);
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

		case STATE_DEATH:
			animator.SetInteger ("state", STATE_DEATH);
			break;
		}

		currentAnimationState = state;
	}
}
