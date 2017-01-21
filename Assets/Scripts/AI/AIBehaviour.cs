using UnityEngine;


/// <summary>
/// Defines a single behaviour for the AI controller. Controls the mob when active.
/// </summary>
public class AIBehaviour {

    protected AIController controller;
    protected Mob mob;

    public AIBehaviour(AIController controller, Mob mob) {
        this.controller = controller;
        this.mob = mob;
    }

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void End() {}

    protected AIBehaviour ShouldAttackOrMove(Vector3 target, float attackRadius,
        AIBehaviour attackBehaviour, AIBehaviour moveBehaviour) {
        var vector = target - mob.transform.position;
        var distanceToTarget = vector.magnitude;

        if (distanceToTarget < attackRadius) {
            var wall = Physics2D.Raycast(mob.transform.position, vector.normalized, distanceToTarget,
                LayerMask.GetMask(Globals.solidLayerName));

            if (wall) {
                return moveBehaviour;
            }
            else {
                return attackBehaviour;
            }
        }
        else {
            return moveBehaviour;
        }
    }

}
