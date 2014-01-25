using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private string gameName = "Lastgate World 1";
    private string typeName = "Lastgate";
    private int maxConnections = 25;
    private int port = 25000;

	// Use this for initialization
    void Start() { startServer(); }
	
	// Update is called once per frame
	void Update () {}

    public void startServer()
    {
        Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("server up");
    }
}
