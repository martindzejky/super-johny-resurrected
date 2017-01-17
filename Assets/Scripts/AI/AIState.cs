/// <summary>
/// A list of states for the AI state machine.
/// </summary>
enum AIState {
    LookingForTargets,
    LookingForGoals,
    MovingTowardsEnemy,
    MovingTowardsGoal,
    AttackingEnemy,
    CapturingGoal,
    Thinking,
    Wandering
};
