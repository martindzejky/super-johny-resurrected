using UnityEngine;


/// <summary>
/// Makes the mob use pathing and move towards the closest goal.
/// </summary>
public class AIMoveTowardsGoal : AIPathingBehaviour
{

    // TODO: Find a better way to keep the path updated
    private float updateTimer = Globals.aiPathingTimer;

    public AIMoveTowardsGoal(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Update() {
        updateTimer -= Time.deltaTime;
        if (controller.GetClosestGoal()) {
            if (UpdateStateBasedOnGoal()) {
                MoveTowardsGoal();
            }
        }
        else {
            controller.ForceUpdateTargets();
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    // TODO: This is a copy of the AIThink's method, refactor
    private bool UpdateStateBasedOnGoal() {
        var vector = controller.GetClosestGoal().transform.position - mob.transform.position;
        var distanceToGoal = vector.magnitude;

        if (distanceToGoal < Globals.aiCaptureRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToGoal, LayerMask.GetMask(Globals.solidLayerName));
            if (wall) {
                return true;
            }
            else {
                controller.SwitchBehaviour(new AICaptureGoal(controller, mob));
                return false;
            }
        }
        else {
            return true;
        }
    }

    private void MoveTowardsGoal() {
        if (updateTimer < 0f || currentPath == null || currentPath.Length == 0) {
            updateTimer = Globals.aiPathingTimer;
            NavigateTowards(controller.GetClosestGoal().transform.position);
        }

        MoveTowardsPathNode();
    }

}

