using UnityEngine;


/// <summary>
/// Handles the logic of dead bodies. Waits a little and then desaturates and burries the body.
/// </summary>
public class DeadBody : MonoBehaviour {

    public enum State {
        Waiting,
        Burrying,
        Burried
    }

    private float rotation;
    private State state = State.Waiting;
    private PhysicsObject physicsObject;
    private SpriteRenderer sprite;

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
        sprite = GetComponent<SpriteRenderer>();
        rotation = Random.Range(2f, 10f) * (Random.value < .5f ? 1 : -1);
    }

    public void Update() {
        switch (state) {
            case State.Waiting:
                UpdateCounter();
                break;

            case State.Burrying:
                Bury();
                break;

            case State.Burried:
                break;
        }
    }

    private void UpdateCounter() {
        if (physicsObject && physicsObject.isGrounded) {
            state = State.Burrying;
            Destroy(physicsObject);
            physicsObject = null;
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
