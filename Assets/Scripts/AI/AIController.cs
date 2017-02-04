using UnityEngine;


/// <summary>
/// Controls mobs. The controller only updates information about the nearby world,
/// the actual behaviour is controlled by the active AI behaviour.
/// It also contains methods to poll its status and change the active behaviour.
/// </summary>
public class AIController : MonoBehaviour {

    // TODO: Add AI personas, the behaviours draw stats and imperfections from active persona

    private Mob myMob;
    private Mob closestEnemy = null;
    private Flag closestGoal = null;
    private float retargetTimer = -1f;
    private bool hadAliveEnemy = false;
    private AIBehaviour activeBehaviour = null;
    private AIPersona persona = null;

    public void Awake() {
        myMob = GetComponent<Mob>();
        var personas = new AIPersona[] {
            new AIPersona(),
            new AIPersona(),
            new AIAttackerPersona(),
            new AICapturerPersona()
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

    public float EnemyPlayerProximityMultiplier(float enemyPlayerDistance, AIPersona persona) {
        return Mathf.Lerp(1f, Mathf.Clamp(enemyPlayerDistance, 1f, 10f) / 4f, persona.PlayerProximityImportance());
    }

    public float GoalPlayerProximityMultiplier(float goalPlayerDistance, AIPersona persona) {
        return Mathf.Lerp(1f, Mathf.Clamp(goalPlayerDistance, 3f, 20f) / 6f, persona.PlayerProximityImportance());
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
        var player = FindObjectOfType<PlayerController>();

        for (uint i = 0; i < numberOfTeams; i++) {
            if (myMob.team == i && i != 0) {
                continue;
            }

            var team = MobTeams.GetTeam(i);
            if (team.mobs.Count > 0) {
                foreach (var enemy in team.mobs) {
                    var distance = (transform.position - enemy.transform.position).sqrMagnitude;

                    if (player) {
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
        var player = FindObjectOfType<PlayerController>();

        foreach (var flag in flags) {
            if (flag.capturedTeam == myMob.team && flag.capturedAmount >= 1f - float.Epsilon) {
                continue;
            }

            var distance = (flag.transform.position - transform.position).sqrMagnitude;

            if (player && player.GetComponent<Mob>().team == myMob.team) {
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
