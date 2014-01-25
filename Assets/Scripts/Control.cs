﻿using UnityEngine;
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

            

			if (Input.GetKey(KeyCode.LeftArrow)) {
                animator.SetInteger("Direction", 2);
				transform.position += new Vector3 (-0.025f, 0f);
			} else if (Input.GetKey(KeyCode.RightArrow)) {
                animator.SetInteger("Direction", 0);
				transform.position += new Vector3 (0.025f, 0f);
			}

			if (Input.GetKey ("up")) {
                animator.SetInteger("Direction", 1);
				transform.position += new Vector3 (0f, 0.025f);
			} else if (Input.GetKey ("down")) {
                animator.SetInteger("Direction", 3);
				transform.position += new Vector3 (0f, -0.025f);
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
