using UnityEngine;


/// <summary>
/// Destroys object when Y position turns too low.
/// </summary>
public class DestroyOutsideLevel : MonoBehaviour {

    public void Update() {
        if (transform.position.y < Globals.destroyLevel) {
            Destroy(gameObject);
        }
    }

}
