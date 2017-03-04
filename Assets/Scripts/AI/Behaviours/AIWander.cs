using UnityEngine;


/// <summary>
/// Makes the mob wander around.
/// </summary>
public class AIWander : AIPathingBehaviour
{

    private float wanderTimer = Globals.aiWanderTimer;
    private float lookTimer = Globals.aiWanderLookTimer;
    private Flag targetGoal = null;
    private float radius;
    private float capturePoint;
    private Vector3 lookTarget;

    public AIWander(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Start() {
        base.Start();

        radius = controller.GetPersona().CaptureRadius();
        capturePoint = Random.Range(-radius, radius);
        lookTarget = mob.transform.position;

        FindNewGoal();
    }

    public override void Update() {
        base.Update();

        UpdateTargetGoal();

        var distance = (mob.transform.position - targetGoal.transform.position).magnitude;
        if (distance > radius) {
            MoveTowardsPathNode();
        }
        else {
            StandNearGoal();
        }
    }

    private void UpdateTargetGoal() {
        if (wanderTimer < 0f || currentPath == null || currentPath.Length == 0) {
            wanderTimer = Globals.aiWanderTimer + Random.Range(-2f, 2f);
            FindNewGoal();
            NavigateTowards(targetGoal.transform.position);
        }
    }

    private void FindNewGoal() {
        var goals = GameObject.FindObjectsOfType<Flag>();
        targetGoal = goals[Random.Range(0, goals.Length)];
    }

    private void StandNearGoal() {
        wanderTimer -= Time.deltaTime;

        var difference = targetGoal.transform.position.x + capturePoint - mob.transform.position.x;
        if (Mathf.Abs(difference) > .8f) {
            mob.Move(Mathf.Sign(difference));
        }

        LookAround();
    }

    private void LookAround() {
        lookTimer -= Time.deltaTime;

        if (lookTimer < 0f) {
            lookTimer = Globals.aiWanderLookTimer + Random.Range(-1f, 1f);
            lookTarget = mob.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
        }

        mob.eyeTarget = lookTarget;
    }

}
