/// <summary>
/// A list of states for the AI state machine.
/// </summary>
enum AIState {
    LookingForTargets,
    LookingForGoals,
    MovingTowardsTarget,
    MovingTowardsGoal,
    AttackingTarget,
    CapturingGoal,
    Wandering
};
