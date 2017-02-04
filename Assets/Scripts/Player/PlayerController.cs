using UnityEngine;


/// <summary>
/// Reads the input from the player and controls a mob.
/// </summary>
public class PlayerController : MonoBehaviour {

    private Mob mob;
    private float idleEyesTimer = 0f;

    public void Awake() {
        mob = GetComponent<Mob>();
    }

    public void Update() {
        var input = Input.GetAxisRaw("Horizontal");

        mob.Move(input);
        if (Input.GetButtonDown("Jump")) {
            mob.Jump();
        }

        if (!Mathf.Approximately(input, 0f)) {
            mob.eyeTarget = transform.position + new Vector3(input * 5f, 0f, 0f);
            idleEyesTimer = Globals.playerIdleEyesTime;
        }
        else {
            idleEyesTimer -= Time.deltaTime;
            if (idleEyesTimer < 0f) {
                mob.eyeTarget = transform.position;
            }
        }
    }

}
