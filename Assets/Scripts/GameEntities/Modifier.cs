using UnityEngine;
using System.Collections;

/// <summary>
/// Peforms modifications of abilities
/// </summary>
public class Modifier {

    public Modifier()
    {
        Field = "None";
        uAmount = 0;
        fAmount = 0f;
        Duration = 0f;
        Applied = false;
    }

    public string Field { get; set; }  // Field that will change
    public uint uAmount { get; set; }  // Amount as either float or uint
    public float fAmount { get; set; }
    public float Duration { get; set; }
    public bool Applied { get; set; }  // Indicates if the modifier has been applied
}
