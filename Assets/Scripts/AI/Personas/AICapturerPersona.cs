using UnityEngine;


/// <summary>
/// This persona makes the mob prefer capturing over attacking.
/// </summary>
public class AICapturerPersona : AIPersona {

    private readonly float importanceRatio;
    private readonly float goalOffset;

    public AICapturerPersona() {
        importanceRatio = Random.Range(.1f, .8f);
        goalOffset = Random.Range(2f, 4f);
    }

    public override float StunnedEnemyPenalty() {
        return base.StunnedEnemyPenalty() * 2f;
    }

    public override float EnemyGoalImportanceRatio() {
        return importanceRatio;
    }

    public override float GoalDistanceOffset() {
        return goalOffset;
    }

    public override float AttackRadius() {
        return 5f;
    }

    public override float CaptureRadius() {
        return 2f;
    }

    public override float ReactionTime() {
        return base.ReactionTime() * 2f;
    }

    public override float PlayerProximityImportance() {
        return .8f;
    }

}

