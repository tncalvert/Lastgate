using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract implementation of items
/// </summary>
public class GameItem : MonoBehaviour {

    public GameItem()
    {
        Name = "Generic Item";
        Type = "Game Item";
        Description = "You should never see this. Oh, no. Game broken. Please help.";
        Value = 0;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public uint Value { get; set; }
    public string Description { get; set; }
}
