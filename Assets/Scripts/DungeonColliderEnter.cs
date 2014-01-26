using UnityEngine;
using System.Collections;

public class DungeonColliderEnter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the trigger " + other.name);
    }
}
