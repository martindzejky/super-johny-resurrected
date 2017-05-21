using UnityEngine;


/// <summary>
/// Adjusts the camera size for pixel-perfect graphics.
/// </summary>
public class PixelCameraSize : MonoBehaviour {

    public uint referenceHeight = 640;

    private int lastSize;

    public void Awake() {
        UpdateCameraSize();
    }

#if UNITY_EDITOR

    public void Update() {
        if (lastSize != Screen.height) {
            UpdateCameraSize();
        }
    }

#endif

    private void UpdateCameraSize() {
        lastSize = Screen.height;

        var refOrthoSize = (float) referenceHeight / Globals.pixelsPerUnit / 2f;
        var orthoSize = (float) lastSize / Globals.pixelsPerUnit / 2f;
        var multiplier = Mathf.Max(1, Mathf.Round(orthoSize / refOrthoSize));

        orthoSize /= multiplier;
        GetComponent<Camera>().orthographicSize = orthoSize * Globals.cameraAdjust;
    }

}
