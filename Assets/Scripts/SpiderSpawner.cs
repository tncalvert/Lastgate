using UnityEngine;
using System.Collections;

public class SpiderSpawner : MonoBehaviour {

	public Transform spiderPrefab;
	public float startTimeout = 0.1f; // Seconds until next spider

	private float timeout;

	// Use this for initialization
	void Start () {
		timeout = startTimeout;
	}
	
	// Update is called once per frame
	void Update () {
		Instantiate(spiderPrefab, new Vector3(-3,0,0), Quaternion.identity); // Change to network
	}
}
