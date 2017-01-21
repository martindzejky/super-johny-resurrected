using UnityEngine;


/// <summary>
/// Defines a single behaviour for the AI controller. Controls the mob when active.
/// </summary>
public class AIBehaviour {

    protected AIController controller;
    protected Mob mob;

    public AIBehaviour(AIController controller, Mob mob) {
        this.controller = controller;
        this.mob = mob;
    }

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    protected AIBehaviour ShouldAttackOrMove(Vector3 target, float attackRadius,
        AIBehaviour attackBehaviour, AIBehaviour moveBehaviour) {
        var vector = target - mob.transform.position;
        var distanceToTarget = vector.magnitude;

        if (distanceToTarget < attackRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToTarget,
                LayerMask.GetMask(Globals.solidLayerName));

            if (wall) {
                return moveBehaviour;
            }
            else {
                return attackBehaviour;
            }
        }
        else {
            return moveBehaviour;
        }
    }

    protected AITarget GetClosestTarget() {
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
                return AITarget.Mob;
            }
            else {
                return AITarget.Goal;
            }
        }
        else if (closestEnemy) {
            return AITarget.Mob;
        }
        else if (closestGoal) {
            return AITarget.Goal;
        }
        else {
            return AITarget.None;
        }
    }

}
