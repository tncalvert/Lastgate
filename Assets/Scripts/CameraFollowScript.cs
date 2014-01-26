using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

	public Transform target = null;
    public float smooth = 5;

    private float minWidth = -4.3f, maxWidth = 4.3f, minHeight = -3.4f, maxHeight = 3.4f;

    void Update ()
    {
        if (target == null)
        {
            return;
        }

        float x = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * smooth);
        float y = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * smooth);

        // Confine camera in town
        // Otherwise the physics of the walls will confine the player
        // They can see the black outside, it doesn't matter
        if (Application.loadedLevelName == "lastgate")
        {
            x = Mathf.Clamp(x, minWidth, maxWidth);
            y = Mathf.Clamp(y, minHeight, maxHeight);
        }

        transform.position = new Vector3(x, y, transform.position.z);

        Debug.Log(transform.position);
    }
}
