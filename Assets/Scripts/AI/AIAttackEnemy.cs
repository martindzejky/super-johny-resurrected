using UnityEngine;


/// <summary>
/// Makes the mob attack the enemy.
/// </summary>
public class AIAttackEnemy : AIBehaviour
{

    public AIAttackEnemy(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Update() {
        if (controller.GetClosestEnemy()) {
            if (UpdateStateBasedOnEnemy()) {
                AttackEnemy();
            }
        }
        else {
            controller.ForceUpdateTargets();
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    // TODO: This is a copy of the AIThink's method, refactor
    private bool UpdateStateBasedOnEnemy() {
        var vector = controller.GetClosestEnemy().transform.position - mob.transform.position;
        var distanceToEnemy = vector.magnitude;

        if (distanceToEnemy < Globals.aiAttackRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToEnemy, LayerMask.GetMask(Globals.solidLayerName));
            if (wall) {
                controller.SwitchBehaviour(new AIMoveTowardsEnemy(controller, mob));
                return false;
            }
            else {
                return true;
            }
        }
        else {
            controller.SwitchBehaviour(new AIMoveTowardsEnemy(controller, mob));
            return false;
        }
    }

    private void AttackEnemy() {
        var closestEnemy = controller.GetClosestEnemy();

        var horizontalDistanceToEnemy = closestEnemy.transform.position.x - mob.transform.position.x;
        var directionToEnemy = Mathf.Sign(horizontalDistanceToEnemy);
        var heightDifference = closestEnemy.transform.position.y - mob.transform.position.y;

        if (heightDifference < 1f) {
            if (Mathf.Abs(horizontalDistanceToEnemy) > .8f) {
                mob.Move(directionToEnemy);
            }
        }
        else {
            if (Mathf.Abs(horizontalDistanceToEnemy) < 2f) {
                mob.Move(-directionToEnemy);
            }
        }

        if (heightDifference > -.5f && heightDifference < .5f) {
            mob.Jump();
        }
    }

}
