using UnityEngine;


/// <summary>
/// This persona makes the mob follow the player and prioritize goals closer to the player.
/// </summary>
public class AIFollowerPersona : AIPersona {

    private float goalOffset;

    public AIFollowerPersona() {
        goalOffset = Random.Range(3f, 6f);
    }

    public override float StunnedEnemyPenalty() {
        return base.StunnedEnemyPenalty() * 2f;
    }

    public override float EnemyGoalImportanceRatio() {
        return 1.2f;
    }

    public override float GoalDistanceOffset() {
        return goalOffset;
    }

    public override float ReactionTime() {
        return base.ReactionTime() * .8f;
    }

    public override float PlayerProximityImportance() {
        return 1f;
    }

}

