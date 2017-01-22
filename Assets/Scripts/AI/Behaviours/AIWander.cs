using UnityEngine;


/// <summary>
/// Makes the mob wander around.
/// </summary>
public class AIWander : AIBehaviour
{

    private float wanderTimer = 0f;
    private float wanderBuffer = 0f;

    public AIWander(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Start() {
        wanderTimer = Random.Range(0f, Globals.aiWanderTimer);
    }

    public override void Update() {
        UpdateTimer();
        Move();
    }

    private void UpdateTimer() {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer < 0f) {
            wanderTimer = Globals.aiWanderTimer;
            wanderBuffer += Random.Range(-Globals.aiWanderMovementTime, Globals.aiWanderMovementTime);
        }
    }

    private void Move() {
        var absWanderBuffer = Mathf.Abs(wanderBuffer);
        if (absWanderBuffer > .2f) {
            var dir = Mathf.Sign(wanderBuffer);
            mob.Move(dir);
            wanderBuffer -= Mathf.Min(dir * Time.deltaTime, absWanderBuffer);

            mob.eyeTarget = mob.transform.position + Vector3.right * 5f * dir;
        }
    }

}
