﻿using UnityEngine;


/// <summary>
/// Defines the attributes of the AI controller and behaviours.
/// Controls various random events like jumping and random importance ratios and chances.
/// </summary>
public class AIPersona {

    private readonly float jumpPoint;
    private readonly float stunnedPenalty;
    private readonly float reactionTime;

    public AIPersona() {
        jumpPoint = Random.Range(-.5f, .5f);
        stunnedPenalty = Random.Range(10f, 16f);
        reactionTime = Random.Range(.3f, .6f);
    }

    public virtual bool ShouldJump() {
        return Random.value < .04f;
    }

    public virtual bool ShouldAccidentalJump() {
        return Random.value < .01f;
    }

    public virtual float JumpPoint() {
        return jumpPoint;
    }

    public virtual float StunnedEnemyPenalty() {
        return stunnedPenalty;
    }

    public virtual float EnemyGoalImportanceRatio() {
        return 1.5f;
    }

    public virtual float GoalDistanceOffset() {
        return 5f;
    }

    public virtual float AttackRadius() {
        return 6f;
    }

    public virtual float CaptureRadius() {
        return 3.5f;
    }

    public virtual float ReactionTime() {
        return reactionTime;
    }

    public virtual float PlayerProximityImportance() {
        return .4f;
    }

}
