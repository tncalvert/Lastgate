using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {
	public Vector2 speed = new Vector2(50, 50);
	private Vector2 movement;

    private Animator animator;
    private List<KeyCode> Horizontal;
    private List<KeyCode> Vertical;

	// Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
        Horizontal = new List<KeyCode>();
        Vertical = new List<KeyCode>();
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (networkView.isMine || (!Network.isServer && !Network.isClient /* Not networked at all */)) {
            // Check new input
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                animator.SetBool("Moving", true);
                animator.SetInteger("Direction", 2);
                Horizontal.Add(KeyCode.LeftArrow);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                animator.SetBool("Moving", true);
                animator.SetInteger("Direction", 0);
                Horizontal.Add(KeyCode.RightArrow);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                animator.SetBool("Moving", true);
                animator.SetInteger("Direction", 1);
                Vertical.Add(KeyCode.UpArrow);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                animator.SetBool("Moving", true);
                animator.SetInteger("Direction", 3);
                Vertical.Add(KeyCode.DownArrow);
            }

            // Prune old input
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Horizontal.RemoveAll(x => x == KeyCode.LeftArrow);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Horizontal.RemoveAll(x => x == KeyCode.RightArrow);
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Vertical.RemoveAll(x => x == KeyCode.UpArrow);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                Vertical.RemoveAll(x => x == KeyCode.DownArrow);
            }

            // Apply movement
            if (Horizontal.Count != 0)
            {
                if(Horizontal[Horizontal.Count - 1] == KeyCode.LeftArrow)
                    transform.position += new Vector3(-0.025f, 0f);
                else if(Horizontal[Horizontal.Count - 1] == KeyCode.RightArrow)
                    transform.position += new Vector3(0.025f, 0f);
            }
            if(Vertical.Count != 0) {
                if (Vertical[Vertical.Count - 1] == KeyCode.UpArrow)
                    transform.position += new Vector3(0f, 0.013f);
                else if (Vertical[Vertical.Count - 1] == KeyCode.DownArrow)
                    transform.position += new Vector3(0f, -0.013f);
            }

            if (Horizontal.Count == 0 && Vertical.Count == 0)
                animator.SetBool("Moving", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("Attack", true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("Attack", false);
            }

            int dir = animator.GetInteger("Direction");
            int att = animator.GetBool("Attack") ? 1 : 0;
            int mv = animator.GetBool("Moving") ? 1 : 0;

            networkView.RPC("UpdateAnimations", RPCMode.Others, dir, att, mv);
            
		}

		// Send update to network
	}

    [RPC]
    public void UpdateAnimations(int dir, int att, int mv)
    {
        
        animator.SetBool("Attack", att == 1);
        animator.SetBool("Moving", mv == 1);
        animator.SetInteger("Direction", dir);
    }

}
