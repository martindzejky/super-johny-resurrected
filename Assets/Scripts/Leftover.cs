using UnityEngine;


/// <summary>
/// Handles the logic of a leftover. This can be for example a dead body or a lost heart.
/// Waits a little and then desaturates and burries the leftover. Or just destroys it on contact.
/// /// </summary>
public class Leftover : MonoBehaviour {

    public enum State {
        Falling,
        Burrying,
        Burried
    }

    public bool initVelocity = true;
    public bool rotate = true;
    public bool bury = false;

    private float rotation;
    private State state = State.Falling;
    private PhysicsObject physicsObject;
    private SpriteRenderer sprite;

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
        sprite = GetComponent<SpriteRenderer>();

        rotation = rotate ? Random.Range(2f, 10f) * (Random.value < .5f ? 1 : -1) : 0f;
        if (initVelocity) {
            physicsObject.velocity = new Vector2(Random.Range(-6f, 6f), Random.Range(10f, 18f));
        }
    }

    public void Update() {
        switch (state) {
            case State.Falling:
                RotateAndDetectCollision();
                break;

            case State.Burrying:
                Bury();
                break;

            case State.Burried:
                break;
        }
    }

    private void RotateAndDetectCollision() {
        if (physicsObject && physicsObject.isGrounded) {
            if (bury) {
                state = State.Burrying;
                Destroy(physicsObject);
                physicsObject = null;
            }
            else {
                Destroy(gameObject);
            }
        }
        else {
            transform.Rotate(Vector3.forward, rotation);
        }
    }

    private void Bury() {
        const float delta = .002f;
        transform.Translate(Vector2.down * .15f * Time.deltaTime, Space.World);
        sprite.color = new Color(sprite.color.r - delta, sprite.color.g - delta, sprite.color.b - delta);

        if (sprite.color.maxColorComponent < .7f) {
            state = State.Burried;
        }
    }

}
