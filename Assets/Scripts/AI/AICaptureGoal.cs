using UnityEngine;


/// <summary>
/// Makes the mob stand still and capture the goal.
/// </summary>
public class AICaptureGoal : AIBehaviour
{

    private float capturePoint;

    public AICaptureGoal(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Start() {
        capturePoint = Random.Range(-Globals.aiCaptureRadius, Globals.aiCaptureRadius);
    }

    public override void Update() {
        if (controller.GetClosestGoal()) {
            if (UpdateStateBasedOnGoal()) {
                CaptureGoal();
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
                controller.SwitchBehaviour(new AIMoveTowardsGoal(controller, mob));
                return false;
            }
            else {
                return true;
            }
        }
        else {
            controller.SwitchBehaviour(new AIMoveTowardsGoal(controller, mob));
            return false;
        }
    }

    private void CaptureGoal() {
        var difference = controller.GetClosestGoal().transform.position.x + capturePoint - mob.transform.position.x;
        if (Mathf.Abs(difference) > .8f) {
            mob.Move(Mathf.Sign(difference));
        }
    }

}

