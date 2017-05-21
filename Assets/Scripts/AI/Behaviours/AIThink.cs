using UnityEngine;


/// <summary>
/// This is the entry behaviour. After a short evaluation period, it looks at the state
/// of the controller and assigns the most appropriate new behaviour.
/// </summary>
public class AIThink : AIBehaviour {

    private float thinkTimer;

    public AIThink(AIController controller, Mob mob) : base(controller, mob) { }

    public override void Start() {
        base.Start();

        thinkTimer = controller.GetPersona().ReactionTime();
        mob.eyeTarget = mob.transform.position + Vector3.up * 5f;
        mob.emotion = MobEmotion.Thinking;
    }

    public override void Update() {
        base.Update();

        thinkTimer -= Time.deltaTime;
        if (thinkTimer < 0f) {
            UpdateStateBasedOnClosestTarget();
        }
    }

    public override void End() {
        base.End();

        mob.emotion = MobEmotion.Normal;
    }

    private void UpdateStateBasedOnClosestTarget() {
        switch (GetClosestTarget()) {
            case AITarget.Enemy:
                UpdateStateBasedOnEnemy();
                break;
            case AITarget.Goal:
                UpdateStateBasedOnGoal();
                break;
            default:
                controller.SwitchBehaviour(new AIWander(controller, mob));
                break;
        }
    }

    private void UpdateStateBasedOnEnemy() {
        controller.SwitchBehaviour(ShouldAttackOrMove(controller.GetClosestEnemy().transform.position,
            controller.GetPersona().AttackRadius(), new AIAttackEnemy(controller, mob),
            new AIMoveTowardsEnemy(controller, mob)));
    }

    private void UpdateStateBasedOnGoal() {
        controller.SwitchBehaviour(ShouldAttackOrMove(controller.GetClosestGoal().transform.position,
            controller.GetPersona().CaptureRadius(), new AICaptureGoal(controller, mob),
            new AIMoveTowardsGoal(controller, mob)));
    }

}
