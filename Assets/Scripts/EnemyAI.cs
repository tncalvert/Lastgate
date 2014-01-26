using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public float speed = 2f;
	private bool isAttacking = false;
	private float attackDuration = 1f;
	private float cooldown = 1f;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isAttacking) { // Let the animation run
			cooldown -= Time.deltaTime;
			if (cooldown <= 0f) {
				isAttacking = false;
				animator.SetBool("Attack", false);
			}
			return;
		}

		GameObject[] players;
		float minDist = Mathf.Infinity;
		GameObject closestPlayer = null;

		players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			float dist = Vector3.Distance(player.transform.position, this.gameObject.transform.position);
			if (dist < minDist) {
				minDist = dist;
				closestPlayer = player;
			}
		}

		if (closestPlayer == null)
			return;

		if (minDist < 0.1f) {
			this.gameObject.GetComponent<GameCharacter>().Attack();
			cooldown = attackDuration;
			this.isAttacking = true;
			animator.SetBool("Moving", false);
			animator.SetBool("Attack", true);
		}
		else {
			float step = speed * Time.deltaTime;
			Vector3 oldPosition = this.gameObject.transform.position;
			Vector3 newPosition = Vector3.MoveTowards(oldPosition, closestPlayer.transform.position, step);
			this.gameObject.transform.position = newPosition;

			animator.SetBool("Moving", true);

			// Set direction for animation
			Vector3 positionDiff = newPosition - oldPosition;
			if (System.Math.Abs(positionDiff.x) >= System.Math.Abs(positionDiff.y)) { /* Primarily moving left/right */
				if (positionDiff.x >= 0) {
					animator.SetInteger("Direction", 2);
				}
				else {
					animator.SetInteger("Direction", 0);
				}
			}
			else {
				if (positionDiff.y >= 0) {
					animator.SetInteger("Direction", 1);
				}
				else {
					animator.SetInteger("Direction", 3);
				}
			}
		}
	}
}
