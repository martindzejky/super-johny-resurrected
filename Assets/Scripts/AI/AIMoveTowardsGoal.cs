/// <summary>
/// Makes the mob use pathing and move towards the closest goal. The path is not
/// updated because the goal is not moving.
/// </summary>
public class AIMoveTowardsGoal : AIPathingBehaviour
{

    public AIMoveTowardsGoal(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Update() {
        base.Update();

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

    protected override void UpdateBasedOnTarget(AITarget closestTarget) {
        if (closestTarget != AITarget.Goal) {
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    private bool UpdateStateBasedOnGoal() {
        var newBehaviour = ShouldAttackOrMove(controller.GetClosestGoal().transform.position,
            Globals.aiCaptureRadius, new AICaptureGoal(controller, mob), this);
        if (newBehaviour == this) {
            return true;
        }
        else {
            controller.SwitchBehaviour(newBehaviour);
            return false;
        }
    }

    private void MoveTowardsGoal() {
        if (currentPath == null || currentPath.Length == 0) {
            NavigateTowards(controller.GetClosestGoal().transform.position);
        }

        MoveTowardsPathNode();
    }

}

