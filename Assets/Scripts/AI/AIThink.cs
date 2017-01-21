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

            if (enemyDistance < goalDistance * Globals.enemyGoalImportanceRatio) {
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
        var vector = controller.GetClosestEnemy().transform.position - mob.transform.position;
        var distanceToEnemy = vector.magnitude;

        if (distanceToEnemy < Globals.aiAttackRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToEnemy, LayerMask.GetMask(Globals.solidLayerName));

            if (wall) {
                controller.SwitchBehaviour(new AIMoveTowardsEnemy(controller, mob));
            }
            else {
                controller.SwitchBehaviour(new AIAttackEnemy(controller, mob));
            }
        }
        else {
            controller.SwitchBehaviour(new AIMoveTowardsEnemy(controller, mob));
        }
    }

    // TODO: Refactor these similar functions into one
    private void UpdateStateBasedOnGoal() {
        var vector = controller.GetClosestGoal().transform.position - mob.transform.position;
        var distanceToGoal = vector.magnitude;

        if (distanceToGoal < Globals.aiCaptureRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToGoal, LayerMask.GetMask(Globals.solidLayerName));

            if (wall) {
                controller.SwitchBehaviour(new AIMoveTowardsGoal(controller, mob));
            }
            else {
                controller.SwitchBehaviour(new AICaptureGoal(controller, mob));
            }
        }
        else {
            controller.SwitchBehaviour(new AIMoveTowardsGoal(controller, mob));
        }
    }

}
