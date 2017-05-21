using UnityEngine;


/// <summary>
/// Defines a single behaviour for the AI controller. Controls the mob when active.
/// </summary>
public class AIBehaviour {

    protected AIController controller;
    protected Mob mob;

    private float targetTimer = Globals.aiClosestTargetTimer;

    public AIBehaviour(AIController controller, Mob mob) {
        this.controller = controller;
        this.mob = mob;
    }

    public virtual void Start() { }

    public virtual void Update() {
        targetTimer -= Time.deltaTime;
        if (targetTimer < 0f) {
            targetTimer = Globals.aiClosestTargetTimer;
            UpdateBasedOnTarget(GetClosestTarget());
        }
    }

    public virtual void End() { }

    protected virtual void UpdateBasedOnTarget(AITarget closestTarget) { }

    protected AIBehaviour ShouldAttackOrMove(Vector3 target, float attackRadius,
        AIBehaviour attackBehaviour, AIBehaviour moveBehaviour) {
        var vector = target - mob.transform.position;
        var distanceToTarget = vector.magnitude;

        if (distanceToTarget < attackRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToTarget,
                LayerMask.GetMask(Globals.solidLayerName));

            return wall ? moveBehaviour : attackBehaviour;
        }

        return moveBehaviour;
    }

    protected AITarget GetClosestTarget() {
        var closestEnemy = controller.GetClosestEnemy();
        var closestGoal = controller.GetClosestGoal();
        var persona = controller.GetPersona();

        if (closestEnemy && closestGoal) {
            var enemyDistance = Vector3.Distance(mob.transform.position, closestEnemy.transform.position);
            var goalDistance = Vector3.Distance(mob.transform.position, closestGoal.transform.position);

            foreach (var playerInfo in MobTeams.GetTeam(mob.team).Players) {
                if (!playerInfo.IsAlive()) {
                    continue;
                }

                var player = playerInfo.mob;

                var enemyPlayerDistance = Vector3.Distance(player.transform.position, closestEnemy.transform.position);
                var goalPlayerDistance = Vector3.Distance(player.transform.position, closestGoal.transform.position);

                enemyDistance *= controller.EnemyPlayerProximityMultiplier(enemyPlayerDistance, persona);
                goalDistance *= controller.GoalPlayerProximityMultiplier(goalPlayerDistance, persona);
            }

            if (closestEnemy.IsStunned()) {
                enemyDistance *= persona.StunnedEnemyPenalty();
            }

            goalDistance = goalDistance * persona.EnemyGoalImportanceRatio() + persona.GoalDistanceOffset();

            return enemyDistance < goalDistance ? AITarget.Enemy : AITarget.Goal;
        }

        if (closestEnemy) {
            return AITarget.Enemy;
        }

        return closestGoal ? AITarget.Goal : AITarget.None;
    }

}
