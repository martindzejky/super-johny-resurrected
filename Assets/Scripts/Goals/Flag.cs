using UnityEngine;


/// <summary>
/// Flag is a goal that can be captured by mobs.
/// </summary>
public class Flag : MonoBehaviour {

    public Transform movingFlag;
    public GameObject[] mobsToSpawn;
    public uint capturedTeam = 0;
    public float capturedAmount = 0f;

    private Collider2D myCollider;
    private float respawnTimer = Globals.respawnTime;

    public void Awake() {
        myCollider = GetComponent<Collider2D>();
    }

    public void Update() {
        CheckCapturing();
        UpdateFlagPosition();
        RespawnMobs();
    }

    private void CheckCapturing() {
        var mobs = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f, LayerMask.GetMask(Globals.mobLayerName));

        foreach (var mobObject in mobs) {
            var mob = mobObject.GetComponent<Mob>();

            // stunned mobs can't capture
            if (mob.IsStunned()) {
                continue;
            }

            if (capturedTeam == mob.team) {
                capturedAmount = Mathf.Min(1f, capturedAmount + Globals.goalCaptureAmountPerMob * Time.deltaTime);
            }
            else {
                capturedAmount -= Globals.goalCaptureAmountPerMob * Time.deltaTime;

                if (capturedAmount < -float.Epsilon) {
                    capturedTeam = mob.team;
                    capturedAmount = 0f;
                    movingFlag.GetComponent<SpriteRenderer>().color = MobTeams.GetTeam(mob.team).teamColor;
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
        if (capturedAmount < 1f - float.Epsilon) {
            respawnTimer = Globals.respawnTime;
        }
        else {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer < 0f) {
                respawnTimer = Globals.respawnTime;

                var spawnX = Random.Range(myCollider.bounds.min.x, myCollider.bounds.max.x);
                var spawnY = transform.position.y;

                if (mobsToSpawn[capturedTeam] && MobTeams.GetTeam(capturedTeam).respawns > 0) {
                    Instantiate(mobsToSpawn[capturedTeam], new Vector2(spawnX, spawnY), Quaternion.identity);
                    MobTeams.GetTeam(capturedTeam).respawns--;
                }
            }
        }
    }

}
