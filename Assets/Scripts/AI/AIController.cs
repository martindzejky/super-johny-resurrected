using UnityEngine;


/// <summary>
/// Controls mobs. The controller only updates information about the nearby world,
/// the actual behaviour is controlled by the active AI behaviour.
/// </summary>
public class AIController : MonoBehaviour {

    // TODO: Refactor into separate AI behaviours
    // ✔. The controller should only update and keep the closest enemy and goal
    // ✔. The controller should store and update an active behaviour
    // ✔. The behaviour should store its state, timers, and a link to the mob and the controller
    // ✔. The behaviour should be able to tell the controller to switch to a different state
    // 5. The closest targets should update periodically and invalidate when dead / captured
    // 6. The path should update only when necessary

    // TODO: Add AI personas, the behaviours draw stats and imperfections from active persona

    private Mob myMob;
    private Mob closestEnemy = null;
    private Flag closestGoal = null;
    private float retargetTimer = 0f;
    private AIBehaviour activeBehaviour;

    public void Awake() {
        myMob = GetComponent<Mob>();
        activeBehaviour = new AIThink(this, myMob);
        activeBehaviour.Start();
    }

    public void Update() {
        UpdateTimers();
        UpdateTargets();
        UpdateBehaviour();
    }

    public void SwitchBehaviour(AIBehaviour newBehaviour) {
        activeBehaviour.End();
        activeBehaviour = newBehaviour;
        activeBehaviour.Start();
    }

    public Mob GetClosestEnemy() {
        return closestEnemy;
    }

    public Flag GetClosestGoal() {
        return closestGoal;
    }

    public void ForceUpdateTargets() {
        retargetTimer = -1f;
        UpdateTargets();
    }

    private void UpdateTimers() {
        retargetTimer -= Time.deltaTime;
    }

    private void UpdateTargets() {
        if (retargetTimer < 0f) {
            retargetTimer = Globals.aiRetargetTimer;
            UpdateClosestMob();
            UpdateClosestGoal();
        }
    }

    private void UpdateBehaviour() {
        activeBehaviour.Update();
    }

    private void UpdateClosestMob() {
        closestEnemy = null;

        var numberOfTeams = MobTeams.GetNumberOfTeams();
        var closestDistance = float.PositiveInfinity;

        for (uint i = 0; i < numberOfTeams; i++) {
            if (myMob.team == i && i != 0) {
                continue;
            }

            var team = MobTeams.GetTeam(i);
            if (team.mobs.Count > 0) {
                foreach (var enemy in team.mobs) {
                    var distance = (transform.position - enemy.transform.position).sqrMagnitude;
                    if (enemy.IsStunned()) {
                        distance *= Globals.aiStunnedEnemyPenalty;
                    }

                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
    }

    private void UpdateClosestGoal() {
        closestGoal = null;

        var closestDistance = float.PositiveInfinity;
        var flags = FindObjectsOfType<Flag>();

        foreach (var flag in flags) {
            if (flag.capturedTeam == myMob.team && flag.capturedAmount >= 1f - float.Epsilon) {
                continue;
            }

            var distance = (flag.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance) {
                closestDistance = distance;
                closestGoal = flag;
            }
        }
    }

}
