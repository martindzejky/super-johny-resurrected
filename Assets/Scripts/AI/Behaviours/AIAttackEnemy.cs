using UnityEngine;


/// <summary>
/// Makes the mob attack the enemy. Tries to stomp the enemy and avoid getting stomped.
/// </summary>
public class AIAttackEnemy : AIBehaviour
{

    public AIAttackEnemy(AIController controller, Mob mob) : base(controller, mob) {}

    public override void Update() {
        base.Update();

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

    protected override void UpdateBasedOnTarget(AITarget closestTarget) {
        if (closestTarget != AITarget.Enemy) {
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    private bool UpdateStateBasedOnEnemy() {
        var newBehaviour = ShouldAttackOrMove(controller.GetClosestEnemy().transform.position,
            controller.GetPersona().AttackRadius(), this, new AIMoveTowardsEnemy(controller, mob));
        if (newBehaviour == this) {
            return true;
        }
        else {
            controller.SwitchBehaviour(newBehaviour);
            return false;
        }
    }

    private void AttackEnemy() {
        var closestEnemy = controller.GetClosestEnemy();
        var persona = controller.GetPersona();

        var horizontalDistanceToEnemy = closestEnemy.transform.position.x - mob.transform.position.x;
        var directionToEnemy = Mathf.Sign(horizontalDistanceToEnemy);
        var heightDifference = closestEnemy.transform.position.y - mob.transform.position.y;

        horizontalDistanceToEnemy += persona.JumpPoint();

        if (heightDifference < 1f) {
            if (Mathf.Abs(horizontalDistanceToEnemy) > .8f) {
                mob.Move(directionToEnemy);
            }
        }
        else {
            if (Mathf.Abs(horizontalDistanceToEnemy) < 3f) {
                mob.Move(-directionToEnemy);
            }
        }

        if (persona.ShouldAccidentalJump()) {
            mob.Jump();
        }

        if (heightDifference > -.5f && heightDifference < .5f && persona.ShouldJump()) {
            mob.Jump();
        }
    }

}
