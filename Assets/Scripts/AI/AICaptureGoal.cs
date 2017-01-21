using UnityEngine;


/// <summary>
/// Makes the mob stand still and capture the goal. Chooses a random capture point.
/// </summary>
public class AICaptureGoal : AIBehaviour
{

    private float capturePoint;

    public AICaptureGoal(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Start() {
        base.Start();

        capturePoint = Random.Range(-Globals.aiCaptureRadius, Globals.aiCaptureRadius);
    }

    public override void Update() {
        base.Update();

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

    protected override void UpdateBasedOnTarget(AITarget closestTarget) {
        if (closestTarget != AITarget.Goal) {
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    private bool UpdateStateBasedOnGoal() {
        var newBehaviour = ShouldAttackOrMove(controller.GetClosestGoal().transform.position,
            Globals.aiCaptureRadius, this, new AIMoveTowardsGoal(controller, mob));
        if (newBehaviour == this) {
            return true;
        }
        else {
            controller.SwitchBehaviour(newBehaviour);
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

