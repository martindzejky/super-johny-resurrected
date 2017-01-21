using UnityEngine;


/// <summary>
/// This is the entry behaviour. After a short evaluation period, it looks at the state
/// of the controller and assigns the most appropriate new behaviour.
/// </summary>
public class AIThink : AIBehaviour {

    private float thinkTimer;

    public AIThink(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Start() {
        thinkTimer = Random.Range(0f, Globals.aiThinkTimer);
    }

    public override void Update() {
        thinkTimer -= Time.deltaTime;
        if (thinkTimer < 0f) {
            UpdateStateBasedOnClosestTarget();
        }
    }

    private void UpdateStateBasedOnClosestTarget() {
        var closestEnemy = controller.GetClosestEnemy();
        var closestGoal = controller.GetClosestGoal();

        if (closestEnemy && closestGoal) {
            var enemyDistance = Vector3.Distance(mob.transform.position, closestEnemy.transform.position);
            var goalDistance = Vector3.Distance(mob.transform.position, closestGoal.transform.position);

            if (closestEnemy.IsStunned()) {
                enemyDistance *= Globals.aiStunnedEnemyPenalty;
            }

            goalDistance *= Globals.enemyGoalImportanceRatio;

            if (enemyDistance < goalDistance) {
                UpdateStateBasedOnEnemy();
            }
            else {
                UpdateStateBasedOnGoal();
            }
        }
        else if (closestEnemy) {
            UpdateStateBasedOnEnemy();
        }
        else if (closestGoal) {
            UpdateStateBasedOnGoal();
        }
        else {
            controller.SwitchBehaviour(new AIWander(controller, mob));
        }
    }

    private void UpdateStateBasedOnEnemy() {
        controller.SwitchBehaviour(ShouldAttackOrMove(controller.GetClosestEnemy().transform.position,
            Globals.aiAttackRadius, new AIAttackEnemy(controller, mob), new AIMoveTowardsEnemy(controller, mob)));
    }

    private void UpdateStateBasedOnGoal() {
        controller.SwitchBehaviour(ShouldAttackOrMove(controller.GetClosestGoal().transform.position,
            Globals.aiCaptureRadius, new AICaptureGoal(controller, mob), new AIMoveTowardsGoal(controller, mob)));
    }

}
