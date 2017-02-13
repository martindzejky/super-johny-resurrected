using UnityEngine;


public class Particle : MonoBehaviour {

    public float timeAlive = .5f;

    private PhysicsObject physicsObject;
    private SpriteRenderer spriteRenderer;
    private Vector2 velocity;
    private float timeLiving = 0f;

    public void Awake() {
        physicsObject = GetComponent<PhysicsObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        transform.localScale = new Vector3(Random.Range(.6f, 1.2f), Random.Range(.6f, 1.2f), 1f);

        if (physicsObject) {
            physicsObject.velocity += velocity;
        }
    }

    public void Update() {
        if (!physicsObject) {
            transform.Translate(velocity * Time.deltaTime, Space.World);
        }

        timeLiving += Time.deltaTime;
        if (timeLiving > timeAlive) {
            Destroy(gameObject);
        }

        var newColor = spriteRenderer.color;
        newColor.a = 1f - timeLiving / timeAlive;
        spriteRenderer.color = newColor;
    }

}
