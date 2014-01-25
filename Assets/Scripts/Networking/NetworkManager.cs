using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    
    private string gameName = "Lastgate World 1";
    private string typeName = "Lastgate";
    private int maxConnections = 25;
    private int port = 25000;
    private HostData[] hostList;

    public GameObject playerPrefab;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // Menu stuff
    // Supports launching a server and finding/connecting as a client
    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(50, 50, 100, 50), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(50, 110, 100, 50), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; ++i)
                {
                    if (GUI.Button(new Rect(200, 50 + (60 * i), 500, 50), hostList[i].gameName + " " + hostList[i].gameType + " " + hostList[i].ip[0] + " " + hostList[i].useNat))
                        JoinServer(hostList[i]);
                }
            }
        }
    }

    // **********************************
    //          SERVER                   
    //***********************************

    // Starts a server at
    // PORT: 25000
    // CONNECTIONS: 25
    public void StartServer()
    {
        Network.InitializeServer(maxConnections, port, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
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
    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    // Responds to events from the master server
    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    // joins a server
    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    // called on connection to server
    void OnConnectedToServer()
    {
        Debug.Log("Connected to Server");
    }

    // called when failed to connect
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Failed to connect to server: " + error.ToString());
    }

}
