using UnityEngine;


/// <summary>
/// Makes the mob use pathing and move towards the closest enemy. The path is periodically updated.
/// </summary>
public class AIMoveTowardsEnemy : AIPathingBehaviour {

    private float updateTimer = Globals.aiPathingTimer;

    public AIMoveTowardsEnemy(AIController controller, Mob mob) : base(controller, mob) { }

    public override void Update() {
        base.Update();

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

    protected override void UpdateBasedOnTarget(AITarget closestTarget) {
        if (closestTarget != AITarget.Enemy) {
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    private bool UpdateStateBasedOnEnemy() {
        var newBehaviour = ShouldAttackOrMove(controller.GetClosestEnemy().transform.position,
            controller.GetPersona().AttackRadius(), new AIAttackEnemy(controller, mob), this);
        if (newBehaviour == this) {
            return true;
        }

        controller.SwitchBehaviour(newBehaviour);
        return false;
    }

    private void MoveTowardsEnemy() {
        if (updateTimer < 0f || currentPath == null || currentPath.Length == 0) {
            updateTimer = Globals.aiPathingTimer;
            NavigateTowards(controller.GetClosestEnemy().transform.position);
        }

        MoveTowardsPathNode();
    }

}
