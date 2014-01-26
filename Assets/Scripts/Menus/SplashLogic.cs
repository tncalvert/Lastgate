using UnityEngine;
using System.Collections;

public class SplashLogic : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		Debug.Log ("FUCK");
		yield return new WaitForSeconds(4f);
		Application.LoadLevel (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
