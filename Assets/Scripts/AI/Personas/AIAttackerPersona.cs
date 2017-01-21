using UnityEngine;


/// <summary>
/// This persona makes the mob prefer attacking over capturing.
/// </summary>
public class AIAttackerPersona : AIPersona {

    private float jumpChance;
    private float importanceRatio;
    private float goalOffset;

    public AIAttackerPersona() {
        jumpChance = Random.Range(.1f, .3f);
        importanceRatio = Random.Range(3f, 6f);
        goalOffset = Random.Range(6f, 15f);
    }

    public override bool ShouldJump() {
        return Random.value < jumpChance;
    }

    public override float JumpPoint() {
        return base.JumpPoint() / 10f;
    }

    public override float StunnedEnemyPenalty() {
        return base.StunnedEnemyPenalty() / 2f;
    }

    public override float EnemyGoalImportanceRatio() {
        return importanceRatio;
    }

    public override float GoalDistanceOffset() {
        return goalOffset;
    }

}
