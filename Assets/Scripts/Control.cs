using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (networkView.isMine || (!Network.isServer && !Network.isClient /* Not networked at all */)) {

            

			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                animator.SetInteger("Direction", 2);
				transform.position += new Vector3 (-1, 0);
			} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                animator.SetInteger("Direction", 0);
				transform.position += new Vector3 (1, 0);
			}

			if (Input.GetKeyDown ("up")) {
                animator.SetInteger("Direction", 1);
				transform.position += new Vector3 (0, 1);
			} else if (Input.GetKeyDown ("down")) {
                animator.SetInteger("Direction", 3);
				transform.position += new Vector3 (0, -1);
			}

            if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.UpArrow))
            {
                animator.SetBool("Moving", true);
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) ||
                Input.GetKeyUp(KeyCode.RightArrow) ||
                Input.GetKeyUp(KeyCode.DownArrow) ||
                Input.GetKeyUp(KeyCode.UpArrow))
            {
                animator.SetBool("Moving", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("Attack", true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("Attack", false);
            }
		}

		// Send update to network
	}
}
