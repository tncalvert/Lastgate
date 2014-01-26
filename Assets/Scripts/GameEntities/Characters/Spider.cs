using UnityEngine;
using System.Collections;

public class Spider : GameCharacter {

	void Awake () {
		init ();
		isEnemy = true;
		Type = "Spider";
		Health = 1;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
