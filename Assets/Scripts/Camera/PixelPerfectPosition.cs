using UnityEngine;


/// <summary>
/// Rounds the position of the object to a pixel boundary,
/// </summary>
public class PixelPerfectPosition : MonoBehaviour {

    public void LateUpdate() {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x * Globals.pixelsPerUnit / Globals.cameraAdjust)
                / Globals.pixelsPerUnit * Globals.cameraAdjust,
            Mathf.Round(transform.position.y * Globals.pixelsPerUnit / Globals.cameraAdjust)
                / Globals.pixelsPerUnit * Globals.cameraAdjust,
            transform.position.z);
    }

}
