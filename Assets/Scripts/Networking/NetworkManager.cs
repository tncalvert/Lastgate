using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    //private string gameName = "Lastgate World 1";
    //private string typeName = "Lastgate";
    private int maxConnections = 25;
    private int port = 25000;
    public HostData[] hostList;

    // Prefabs
    public GameObject dwarfPrefab;
    public GameObject magePrefab;
    public GameObject spiderPrefab;
    public GameObject slimePrefab;
    public GameObject fireballPrefab;

	public Transform localPlayer;
	public Transform remotePlayer;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() { }

    // Menu stuff
    // Supports launching a server and finding/connecting as a client
    void OnGUI()
    {
        //if (!Network.isClient && !Network.isServer)
        //{
        //    if (GUI.Button(new Rect(50, 50, 100, 50), "Start Server"))
        //        StartServer();

        //    if (GUI.Button(new Rect(50, 110, 100, 50), "Refresh Hosts"))
        //        RefreshHostList();

        //    if (hostList != null)
        //    {
        //        for (int i = 0; i < hostList.Length; ++i)
        //        {
        //            if (GUI.Button(new Rect(200, 50 + (60 * i), 500, 50), hostList[i].gameName + " " + hostList[i].gameType + " " + hostList[i].ip[0] + " " + hostList[i].useNat))
        //                JoinServer(hostList[i]);
        //        }
        //    }
        //}
    }

    // **********************************
    //          SERVER                   
    //***********************************

    // Starts a server at
    // PORT: 25000
    // CONNECTIONS: 25
    public void StartServer(string typeName, string gameName)
    {
        Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    public void StopServer()
    {
        Network.Disconnect();
        MasterServer.UnregisterHost();
    }

    // Called when server is initialized
    void OnServerInitialized()
    {
        Debug.Log("server up");
    }


    //************************************
    //            CLIENT
    //************************************

    // Gets updated host list
    public void RefreshHostList(string typeName)
    {
        MasterServer.RequestHostList(typeName);
    }

    // Responds to events from the master server
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

	// Immediately destroy and remove RPC for network instantiated object
	// (script attached to prefab)
//	private void OnNetworkInstantiate(NetworkMessageInfo info) {
//		Debug.Log(networkView.viewID + " spawned");
//		if (Network.isServer) {
//			Network.RemoveRPCs(networkView.viewID);
//			Network.Destroy(gameObject);
//		}
//	}

    // joins a server
    public void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    // called on connection to server
    void OnConnectedToServer()
	{
		Debug.Log("Connected to Server");

		// Create character
		Network.Instantiate(localPlayer, new Vector3(0,0,0), Quaternion.identity, 0);

//		// Create others characters
//		for (int i = 0; i < Network.connections.Length; i++) {
//			Transform p = Network.Instantiate(localPlayer, new Vector3(0,0,0), Quaternion.identity, 0) as Transform; // change to remote player
//			p.transform.position = new Vector3(i, 0, 0);
//		}
    }

    // called when failed to connect
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Failed to connect to server: " + error.ToString());
    }

}
