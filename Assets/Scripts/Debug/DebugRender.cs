using UnityEngine;


/// <summary>
/// Just shows or hides based on debugging.
/// </summary>
public class DebugRender : MonoBehaviour {

    public void Awake() {
        GetComponent<SpriteRenderer>().enabled = Globals.debugging;
    }

}
