using UnityEngine;


/// <summary>
/// Makes the mob use pathing and move towards the closest enemy
/// </summary>
public class AIMoveTowardsEnemy : AIPathingBehaviour
{

    // TODO: Find a better way to keep the path updated
    private float updateTimer = Globals.aiPathingTimer;

    public AIMoveTowardsEnemy(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Update() {
        updateTimer -= Time.deltaTime;
        if (controller.GetClosestEnemy()) {
            if (UpdateStateBasedOnEnemy()) {
                MoveTowardsEnemy();
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
                return true;
            }
            else {
                controller.SwitchBehaviour(new AIAttackEnemy(controller, mob));
                return false;
            }
        }
        else {
            return true;
        }
    }

    private void MoveTowardsEnemy() {
        if (updateTimer < 0f || currentPath == null || currentPath.Length == 0) {
            updateTimer = Globals.aiPathingTimer;
            NavigateTowards(controller.GetClosestEnemy().transform.position);
        }

        MoveTowardsPathNode();
    }

}
