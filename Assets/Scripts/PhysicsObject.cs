using UnityEngine;


/// <summary>
/// Allows an object to move while avoiding collisions.
/// If enabled, controls the physics of the object and applies gravity.
/// </summary>
public class PhysicsObject : MonoBehaviour {

    public bool isDynamic = true;
    public Vector2 size = new Vector2(1f, 1f);
    public Vector2 velocity = new Vector2();

    public bool isGrounded {
        get { return collisions.bottom; }
    }

    private CollisionInfo collisions = new CollisionInfo();

    public void Update() {
        if (isDynamic) {
            velocity.y -= Globals.gravity * Time.deltaTime;
        }

        Move();

        // apply ground friction
        // if (isGrounded) {
        //     velocity.x = 0f;
        // }
    }

    private void ResetCollisions() {
        collisions.top = false;
        collisions.right = false;
        collisions.bottom = false;
        collisions.left = false;
    }

    private Vector2 GetBoxcastSize() {
        var boxCastSize = size;
        boxCastSize.x -= Globals.skinThickness * 2f;
        boxCastSize.y -= Globals.skinThickness * 2f;
        return boxCastSize;
    }

    private void Move() {
        ResetCollisions();
        MoveHorizontally();
        MoveVertically();
    }

    private void MoveHorizontally() {
        var motion = velocity.x * Time.deltaTime;
        var movingRight = motion >= 0f;

        // cast a box to see if there are any collisions
        var collision = Physics2D.BoxCast(transform.position, GetBoxcastSize(), 0, movingRight ? Vector2.right : Vector2.left,
            Mathf.Abs(motion) + Globals.skinThickness, 1 << LayerMask.NameToLayer(Globals.solidLayerName));

        if (collision) {
            // restrict movement and move to contact with the collider
            motion = (collision.distance - Globals.skinThickness) * Mathf.Sign(motion);
            velocity.x = 0f;

            if (movingRight) {
                collisions.right = true;
            }
            else {
                collisions.left = true;
            }
        }

        transform.Translate(Vector2.right * motion);
    }

    private void MoveVertically() {
        var motion = velocity.y * Time.deltaTime;
        var movingUp = motion >= 0f;

        // cast a box to see if there are any collisions
        var collision = Physics2D.BoxCast(transform.position, GetBoxcastSize(), 0, movingUp ? Vector2.up : Vector2.down,
            Mathf.Abs(motion) + Globals.skinThickness, 1 << LayerMask.NameToLayer(Globals.solidLayerName));

        if (collision) {
            // restrict movement and move to contact with the collider
            motion = (collision.distance - Globals.skinThickness) * Mathf.Sign(motion);
            velocity.y = 0f;

            if (movingUp) {
                collisions.top = true;
            }
            else {
                collisions.bottom = true;
            }
        }

        transform.Translate(Vector2.up * motion);
    }

}
