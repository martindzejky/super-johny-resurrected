using UnityEngine;


/// <summary>
/// Reads the input from the player and controls a mob.
/// </summary>
public class PlayerController : MonoBehaviour {

    private Mob mob;

    public void Awake() {
        mob = GetComponent<Mob>();
    }

    public void Update() {
        mob.Move(Input.GetAxisRaw("Horizontal"));
        if (Input.GetButtonDown("Jump")) {
            mob.Jump();
        }
    }

}
