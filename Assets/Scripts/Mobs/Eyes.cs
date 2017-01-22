using UnityEngine;


/// <summary>
/// Controls the movement of the mob's eyes.
/// </summary>
public class Eyes : MonoBehaviour {

    private Mob mob;
    private Vector3 offset;

    public void Awake() {
        mob = transform.parent.GetComponent<Mob>();
        offset = transform.localPosition;
    }

    public void Update() {
        Vector3 targetPosition;
        if (mob.IsStunned()) {
            targetPosition = Vector3.zero;
        }
        else {
            var vector = mob.eyeTarget - mob.transform.position;
            vector.z = 0f;
            vector.Normalize();
            targetPosition = new Vector3(vector.x * Globals.maxEyeXOffset, vector.y * Globals.maxEyeYOffset, 0f);
        }

        var delta = targetPosition - transform.localPosition + offset;
        transform.Translate(delta * Globals.eyeFollowSpeed * Time.deltaTime);
    }

}
