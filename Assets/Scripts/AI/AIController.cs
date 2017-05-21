using UnityEngine;


/// <summary>
/// Controls mobs. The controller only updates information about the nearby world,
/// the actual behaviour is controlled by the active AI behaviour.
/// It also contains methods to poll its status and change the active behaviour.
/// </summary>
public class AIController : MonoBehaviour {

    private Mob myMob;
    private Mob closestEnemy;
    private Flag closestGoal;
    private float retargetTimer = -1f;
    private bool hadAliveEnemy;
    private AIBehaviour activeBehaviour;
    private AIPersona persona;

    public void Awake() {
        myMob = GetComponent<Mob>();
        var defaultPersona = new AIPersona();
        var personas = new[] {
            defaultPersona,
            defaultPersona,
            new AIAttackerPersona(),
            new AICapturerPersona(),
            new AIFollowerPersona()
        };
        persona = personas[Random.Range(0, personas.Length)];
        activeBehaviour = new AIThink(this, myMob);
        activeBehaviour.Start();
    }

    public void Update() {
        UpdateTimers();
        UpdateTargets();
        UpdateBehaviour();
    }

    public void OnDestroy() {
        activeBehaviour.End();
        activeBehaviour = null;
    }

    public void SwitchBehaviour(AIBehaviour newBehaviour) {
        if (newBehaviour == activeBehaviour) {
            return;
        }

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

    public AIPersona GetPersona() {
        return persona;
    }

    public void ForceUpdateTargets() {
        retargetTimer = -1f;
        UpdateTargets();
    }

    public float EnemyPlayerProximityMultiplier(float enemyPlayerDistance, AIPersona forPersona) {
        return Mathf.Lerp(1f, Mathf.Clamp(enemyPlayerDistance, 1f, 10f) / 4f, forPersona.PlayerProximityImportance());
    }

    public float GoalPlayerProximityMultiplier(float goalPlayerDistance, AIPersona forPersona) {
        return Mathf.Lerp(1f, Mathf.Clamp(goalPlayerDistance, 3f, 20f) / 6f, forPersona.PlayerProximityImportance());
    }

    private void UpdateTimers() {
        retargetTimer -= Time.deltaTime;
    }

    private void UpdateTargets() {
        if (retargetTimer < 0f || (!closestEnemy && hadAliveEnemy) || (closestGoal && closestGoal.IsCapturedByTeam(myMob.team))) {
            retargetTimer = Globals.aiRetargetTimer;
            UpdateClosestEnemy();
            UpdateClosestGoal();

            hadAliveEnemy = closestEnemy;
        }
    }

    private void UpdateBehaviour() {
        activeBehaviour.Update();
    }

    private void UpdateClosestEnemy() {
        closestEnemy = null;

        var numberOfTeams = MobTeams.GetNumberOfTeams();
        var closestDistance = float.PositiveInfinity;

        for (uint i = 0; i < numberOfTeams; i++) {
            if (myMob.team == i && i != 0) {
                continue;
            }

            var team = MobTeams.GetTeam(i);
            if (team.Mobs.Count > 0) {
                foreach (var enemy in team.Mobs) {
                    var distance = (transform.position - enemy.transform.position).sqrMagnitude;

                    foreach (var playerInfo in MobTeams.GetTeam(myMob.team).Players) {
                        if (!playerInfo.IsAlive()) {
                            continue;
                        }

                        var player = playerInfo.mob;
                        var playerDistance = Vector3.Distance(player.transform.position, enemy.transform.position);
                        distance *= EnemyPlayerProximityMultiplier(playerDistance, persona);
                    }

                    if (enemy.IsStunned()) {
                        distance *= persona.StunnedEnemyPenalty();
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
            if (flag.locked || (flag.capturedTeam == myMob.team && flag.capturedAmount >= 1f - float.Epsilon)) {
                continue;
            }

            var distance = (flag.transform.position - transform.position).sqrMagnitude;

            foreach (var playerInfo in MobTeams.GetTeam(myMob.team).Players) {
                if (!playerInfo.IsAlive()) {
                    continue;
                }

                var player = playerInfo.mob;
                var playerDistance = Vector3.Distance(player.transform.position, flag.transform.position);
                distance *= GoalPlayerProximityMultiplier(playerDistance, persona);
            }

            if (distance < closestDistance) {
                closestDistance = distance;
                closestGoal = flag;
            }
        }
    }

}
