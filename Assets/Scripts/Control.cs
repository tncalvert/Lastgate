using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (networkView.isMine || (!Network.isServer && !Network.isClient /* Not networked at all */)) {
			if (Input.GetKey ("left")) {
				transform.position += new Vector3 (-1, 0);
			} else if (Input.GetKey ("right")) {
				transform.position += new Vector3 (1, 0);
			}

			if (Input.GetKey ("up")) {
				transform.position += new Vector3 (0, 1);
			} else if (Input.GetKey ("down")) {
				transform.position += new Vector3 (0, -1);
			}
		}

		// Send update to network
	}
}
