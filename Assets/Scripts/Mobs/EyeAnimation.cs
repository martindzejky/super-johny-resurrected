﻿using UnityEngine;


/// <summary>
/// Updates the parameters of the animator according to the state of the mob.
/// </summary>
public class EyeAnimation : MonoBehaviour {

    private Animator animator;
    private Mob mob;

    public void Awake() {
        animator = GetComponent<Animator>();
        mob = transform.parent.GetComponent<Mob>();
    }

    public void Update() {
        animator.SetBool("Stunned", mob.IsStunned());
        animator.SetBool("Thinking", mob.emotion == MobEmotion.Thinking);
        animator.SetBool("Shocked", mob.emotion == MobEmotion.Shocked);
        animator.SetBool("Happy", mob.emotion == MobEmotion.Happy);
        animator.SetBool("Scared", mob.emotion == MobEmotion.Scared);
    }

}
