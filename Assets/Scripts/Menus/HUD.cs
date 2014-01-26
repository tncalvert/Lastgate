using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    private bool showMenu;

    void Awake()
    {
        showMenu = false;
    }

    void OnGUI()
    {
        if (!showMenu)
        {
            if (GUI.Button(new Rect(Screen.width - 50, Screen.height - 30, 45, 25), "Menu"))
                showMenu = true;
        }
        else
        {
            if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 50), "Disconnect"))
            {
                Network.CloseConnection(Network.connections[0], true);
                Application.LoadLevel("mainmenu");
            }

            if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 40, 100, 50), "Quit"))
                Application.Quit();

            if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 30, 100, 50), "Close"))
                showMenu = false;
        }
    }
}
