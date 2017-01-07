using UnityEngine;


/**
 * Defines the behaviour of a mob, its movement and controls.
 */
public class Mob : MonoBehaviour {

    private PhysicsObject physicsObject;
    private float horizontalInput = 0f;
    private bool jump = false;

    public void SetHorizontalInput(float input) {
        horizontalInput = input;
    }

    public void Jump() {
        jump = true;
    }

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
    }

    public void Update() {
        physicsObject.velocity.x = horizontalInput * Globals.mobMoveSpeed;

        if (jump) {
            physicsObject.velocity.y = Globals.mobJumpStrength;
        }

        horizontalInput = 0;
        jump = false;
    }

}
