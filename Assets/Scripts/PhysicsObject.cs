using UnityEngine;


/**
 * Controls the physics of an object.
 */
public class PhysicsObject : MonoBehaviour {

    public Vector2 velocity = new Vector2();

    public void Update() {
        velocity.y -= Globals.gravity * Time.deltaTime;

        var motion = velocity * Time.deltaTime;

        // cast a box to see if we would hit anything
        var result = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y),
            new Vector2(1, 1), 0, motion, motion.magnitude, 1 << LayerMask.NameToLayer(Globals.staticLayerName));

        if (result.collider) {
            velocity = motion = new Vector2();
        }

        transform.Translate(motion);
    }

}
