using UnityEngine;


/// <summary>
/// Flag is a goal that can be captured by mobs.
/// </summary>
public class Flag : MonoBehaviour {

    public Transform movingFlag;
    public uint capturedTeam = 0;
    public float capturedAmount = 0f;
    public bool locked = false;

    private Collider2D myCollider;
    private float respawnTimer = Globals.respawnTime;
    private bool scoredForCapturing = false;

    public void Awake() {
        myCollider = GetComponent<Collider2D>();
    }

    public void Start() {
        movingFlag.GetComponent<SpriteRenderer>().color = MobTeams.GetTeam(capturedTeam).teamColor;
    }

    public void Update() {
        CheckCapturing();
        UpdateFlagPosition();
        RespawnMobs();
    }

    public bool IsCapturedByTeam(uint team) {
        return capturedTeam == team && capturedAmount >= 1f - float.Epsilon;
    }

    public GameObject SpawnMob(GameObject prefab) {
        var spawnX = Random.Range(myCollider.bounds.min.x, myCollider.bounds.max.x);
        var spawnY = transform.position.y;
        var spawn = new Vector2(spawnX, spawnY);

        return Instantiate(prefab, spawn, Quaternion.identity);
    }

    private void CheckCapturing() {
        if (locked) {
            return;
        }

        var mobs = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f, LayerMask.GetMask(Globals.mobLayerName));

        foreach (var mobObject in mobs) {
            var mob = mobObject.GetComponent<Mob>();

            // stunned mobs can't capture
            if (mob.IsStunned()) {
                continue;
            }

            if (capturedTeam == mob.team) {
                capturedAmount = Mathf.Min(1f, capturedAmount + Globals.goalCaptureAmountPerMob * Time.deltaTime);

                if (capturedAmount >= 1f - float.Epsilon && !scoredForCapturing) {
                    scoredForCapturing = true;
                    MobTeams.GetTeam(mob.team).score += 5;

                    var prefabRegistry = FindObjectOfType<PrefabRegistry>();
                    var scoreText = Instantiate(prefabRegistry.floatingText, transform.position, transform.rotation);
                    scoreText.GetComponent<FloatingText>().SetText("+5");
                }
            }
            else {
                capturedAmount -= Globals.goalCaptureAmountPerMob * Time.deltaTime;

                if (capturedAmount < -float.Epsilon) {
                    capturedTeam = mob.team;
                    capturedAmount = 0f;
                    movingFlag.GetComponent<SpriteRenderer>().color = MobTeams.GetTeam(mob.team).teamColor;
                    scoredForCapturing = false;
                }
            }
        }
    }

    private void UpdateFlagPosition() {
        var newPosition = movingFlag.localPosition;
        newPosition.y = capturedAmount * 3.8f;
        movingFlag.localPosition = newPosition;
    }

    private void RespawnMobs() {
        if (capturedAmount < .9f) {
            respawnTimer = Globals.respawnTime;
        }
        else {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer < 0f) {
                respawnTimer = Globals.respawnTime;

                var prefabRegistry = FindObjectOfType<PrefabRegistry>();
                if (prefabRegistry.teamMobs[capturedTeam] && MobTeams.GetTeam(capturedTeam).respawns > 0) {
                    MobTeams.GetTeam(capturedTeam).respawns--;
                    SpawnMob(prefabRegistry.teamMobs[capturedTeam]);
                }
            }
        }
    }

}
