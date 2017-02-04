using UnityEngine;


/// <summary>
/// Reads the input from the player and controls a mob.
/// </summary>
public class PlayerController : MonoBehaviour {

    private Mob mob;
    private Flag closestGoal;
    private float idleEyesTimer = 0f;
    private float retargetGoalTimer = 0f;

    public void Awake() {
        mob = GetComponent<Mob>();
    }

    public void Update() {
        var input = Input.GetAxisRaw("Horizontal");

        mob.Move(input);
        if (Input.GetButtonDown("Jump")) {
            mob.Jump();
        }

        UpdateClosestGoal();
        MoveEyes(input);
    }

    private void UpdateClosestGoal() {
        retargetGoalTimer -= Time.deltaTime;
        if (retargetGoalTimer < 0f) {
            retargetGoalTimer = Globals.aiRetargetTimer;

            closestGoal = null;
            var closestDistance = float.PositiveInfinity;

            var goals = FindObjectsOfType<Flag>();
            foreach (var goal in goals) {
                var distance = (transform.position - goal.transform.position).sqrMagnitude;
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestGoal = goal;
                }
            }
        }
    }

    private void MoveEyes(float input) {
        if (!Mathf.Approximately(input, 0f)) {
            mob.eyeTarget = transform.position + new Vector3(input * 5f, 0f, 0f);
            idleEyesTimer = Globals.playerIdleEyesTime;
        }
        else {
            idleEyesTimer -= Time.deltaTime;
            if (idleEyesTimer < 0f) {
                if (closestGoal && Vector3.Distance(transform.position, closestGoal.transform.position) < 5f) {
                    mob.eyeTarget = closestGoal.movingFlag.position;
                }
                else {
                    mob.eyeTarget = transform.position;
                }
            }
        }
    }

}
