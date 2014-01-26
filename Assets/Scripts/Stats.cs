using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	private GameCharacter player;

	// Use this for initialization
	void Start () {
		player = this.GetComponent<GameCharacter> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		GUI.Label (new Rect (4, 4, 100, 100), "Health: " + player.Health + "\nKills: " + player.Kills); 
	}
}
