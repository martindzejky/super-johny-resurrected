using UnityEngine;


/**
 * Reads the input and controls a mob.
 */
public class PlayerController : MonoBehaviour {

    private Mob mob;

    public void Awake() {
        mob = GetComponent<Mob>();
    }

    public void Update() {
        mob.SetHorizontalInput(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButtonDown("Jump")) {
            mob.Jump();
        }
    }

}
