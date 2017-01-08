using UnityEngine;


public class PixelCameraSize : MonoBehaviour {

    private int lastSize;
    private const uint referenceHeight = 320;

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

        float refOrthoSize = (float) referenceHeight / Globals.pixelsPerUnit / 2f;
        float orthoSize = (float) lastSize / Globals.pixelsPerUnit / 2f;
        float multiplier = Mathf.Max(1, Mathf.Round(orthoSize / refOrthoSize));

        orthoSize /= multiplier;
        GetComponent<Camera>().orthographicSize = orthoSize;
    }

}
