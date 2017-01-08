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
        if (!Mathf.Approximately(0f, horizontalInput)) {
            var acceleration = horizontalInput * Globals.mobMaxMoveSpeed / Globals.mobAccelerationTime * Time.deltaTime;

            if (Mathf.Sign(physicsObject.velocity.x) == Mathf.Sign(horizontalInput)) {
                physicsObject.velocity.x += acceleration;
            }
            else {
                physicsObject.velocity.x += acceleration * 2;
            }

            physicsObject.velocity.x = Mathf.Clamp(physicsObject.velocity.x, -Globals.mobMaxMoveSpeed, Globals.mobMaxMoveSpeed);
            physicsObject.applyGroundFriction = false;
            physicsObject.applyAirFriction = false;
        }
        else {
            physicsObject.applyGroundFriction = true;
            physicsObject.applyAirFriction = true;
        }

        if (jumpInput && physicsObject.isGrounded) {
            physicsObject.velocity.y = CalculateVelocityForJumpHeight(Globals.mobJumpHeight);
        }
        jumpInput = false;
    }

    private float CalculateVelocityForJumpHeight(float height) {
        return Mathf.Sqrt(2f * height * Globals.gravity);
    }

}
