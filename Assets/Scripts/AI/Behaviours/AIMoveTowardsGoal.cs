﻿using UnityEngine;


/// <summary>
/// Makes the mob use pathing and move towards the closest goal. The path is not
/// updated because the goal is not moving.
/// </summary>
public class AIMoveTowardsGoal : AIPathingBehaviour {

    private float updateTimer = Globals.aiPathingTimer;

    public AIMoveTowardsGoal(AIController controller, Mob mob) : base(controller, mob) { }

    public override void Update() {
        base.Update();

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

    protected override void UpdateBasedOnTarget(AITarget closestTarget) {
        if (closestTarget != AITarget.Goal) {
            controller.SwitchBehaviour(new AIThink(controller, mob));
        }
    }

    private bool UpdateStateBasedOnGoal() {
        var newBehaviour = ShouldAttackOrMove(controller.GetClosestGoal().transform.position,
            controller.GetPersona().CaptureRadius(), new AICaptureGoal(controller, mob), this);
        if (newBehaviour == this) {
            return true;
        }

        controller.SwitchBehaviour(newBehaviour);
        return false;
    }

    private void MoveTowardsGoal() {
        if (updateTimer < 0f || currentPath == null || currentPath.Length == 0) {
            updateTimer = Globals.aiPathingTimer;
            NavigateTowards(controller.GetClosestGoal().transform.position);
        }

        MoveTowardsPathNode();
    }

}
