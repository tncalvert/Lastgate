using UnityEngine;
using System.Collections;

/// <summary>
/// Used to display and handle the main menu
/// </summary>
public class MainMenu : MonoBehaviour {

    /// <summary>
    /// Controls which menu is showing
    /// 0 = main menu
    /// 1 = client connecting
    /// 2 = setup server
    /// </summary>
    private uint screen;
    private string typeName = "";
    private string gameName = "";
    private string findGameTypeName = "";
    public NetworkManager networkManager;

    void Awake()
    {
        screen = 0;
    }

    void OnGUI()
    {
        if (screen == 0)
        {
            if (Network.isServer)
                GUI.enabled = false;
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Play"))
                screen = 1;
            GUI.enabled = true;

            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 35, 100, 50), "Start Server"))
                screen = 2;

            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 95, 100, 50), "Quit"))
                Application.Quit();
        }
        else if (screen == 1)
        {
            GUI.Label(new Rect(Screen.width - 460, Screen.height - 95, 50, 25), "Game Name");
            findGameTypeName = GUI.TextField(new Rect(Screen.width - 400, Screen.height - 95, 100, 25), findGameTypeName);
            if (GUI.Button(new Rect(Screen.width - 400, Screen.height - 60, 100, 50), "Refresh Hosts"))
                networkManager.RefreshHostList(findGameTypeName);

            if (networkManager.hostList != null)
            {
                for (int i = 0; i < networkManager.hostList.Length; ++i)
                {
                    if (GUI.Button(new Rect(Screen.width / 2 - 75, 100 + (i * 65), 150, 50), networkManager.hostList[i].gameName))
                    {
                        networkManager.JoinServer(networkManager.hostList[i]);
                        Application.LoadLevel("lastgate");
                    }
                }
            }

            if (GUI.Button(new Rect(Screen.width - 275, Screen.height - 60, 100, 50), "Back"))
                screen = 0;

            if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 60, 100, 50), "Quit"))
                Application.Quit();
        }
        else if (screen == 2)
        {
            
            if (!Network.isServer && !Network.isClient)
            {
                GUI.Label(new Rect(Screen.width / 2 - 55, Screen.height / 4, 50, 25), "Game Name");
                typeName = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 4, 200, 25), typeName);
                GUI.Label(new Rect(Screen.width / 2 - 55, Screen.height / 4 + 35, 50, 25), "World Name");
                gameName = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 4 + 35, 200, 25), gameName);


                if (GUI.Button(new Rect(Screen.width / 2 - 55, Screen.height / 4 + 75, 100, 50), "Create"))
                    networkManager.StartServer(typeName, gameName);
            }
            else if (Network.isServer)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Stop Server"))
                    networkManager.StopServer();
            }

            if (GUI.Button(new Rect(Screen.width - 275, Screen.height - 60, 100, 50), "Back"))
                screen = 0;

            if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 60, 100, 50), "Quit"))
                Application.Quit();
        }
    }
}
