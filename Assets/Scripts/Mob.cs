using UnityEngine;


/// <summary>
/// Defines mob behaviour, physics, and stats. Uses the physics
/// object and enables other components to control the mob.
/// </summary>
public class Mob : MonoBehaviour {

    private PhysicsObject physicsObject;
    private float horizontalInput = 0f;
    private bool jumpInput = false;

    public void Move(float direction) {
        horizontalInput = direction;
    }

    public void Stop() {
        horizontalInput = 0f;
    }

    public void Jump() {
        jumpInput = true;
    }

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
    }

    public void Update() {
        // adjust the mob's velocity according to the input
        physicsObject.velocity.x = horizontalInput * Globals.mobMoveSpeed;

        if (jumpInput) {
            jumpInput = false;

            if (physicsObject.isGrounded) {
                physicsObject.velocity.y = CalculateVelocityForJumpHeight(Globals.mobJumpHeight);
            }
        }
    }

    private float CalculateVelocityForJumpHeight(float height) {
        return Mathf.Sqrt(2f * height * Globals.gravity);
    }

}
