using UnityEngine;


/// <summary>
/// Flag is a goal that can be captured by mobs. Handles the logic of capturing
/// and updating the waving flag.
/// </summary>
public class Flag : MonoBehaviour {

    public Transform movingFlag;
    public bool locked;

    public float CapturedAmount => capturedAmount;
    public uint CapturedTeam  => capturedTeam;

    private Collider2D myCollider;
    private bool scoredForCapturing;

    private float capturedAmount;
    private uint capturedTeam;
    private int previousCapturedTeam = -1;

    public void Awake() {
        myCollider = GetComponent<Collider2D>();
    }

    public void Start() {
        movingFlag.GetComponent<SpriteRenderer>().color = MobTeams.GetTeam(capturedTeam).teamColor;
    }

    public void Update() {
        CheckCapturing();
        UpdateFlagPosition();
    }

    public void Capture(uint team) {
        capturedAmount = 1f;
        capturedTeam = team;
        previousCapturedTeam = (int) team;
    }

    public GameObject SpawnMob(GameObject prefab) {
        var spawnX = Random.Range(myCollider.bounds.min.x, myCollider.bounds.max.x);
        var spawnY = transform.position.y;
        var spawn = new Vector2(spawnX, spawnY);

        return Instantiate(prefab, spawn, Quaternion.identity);
    }


    public bool IsCapturedByTeam(uint team) {
        return previousCapturedTeam == capturedTeam && capturedTeam == team;
    }

    private void CheckCapturing() {
        if (locked) {
            return;
        }

        var mobs = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f,
            LayerMask.GetMask(Globals.mobLayerName));

        var someoneCapturing = false;
        foreach (var mobObject in mobs) {
            var mob = mobObject.GetComponent<Mob>();

            // stunned mobs can't capture
            if (mob.IsStunned()) {
                continue;
            }

            someoneCapturing = true;

            if (capturedTeam == mob.team) {
                capturedAmount = Mathf.Min(1f, capturedAmount + Globals.goalCaptureAmountPerMob * Time.deltaTime);

                if (capturedAmount >= 1f - float.Epsilon && !scoredForCapturing) {
                    scoredForCapturing = true;
                    previousCapturedTeam = (int) capturedTeam;
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

        // if noone is capturing, slowly decrease the amount if not fully captured
        if (!someoneCapturing) {
            if (capturedTeam == previousCapturedTeam) {
                capturedAmount = Mathf.Min(1f, capturedAmount + Globals.goalCaptureAmountAlone * Time.deltaTime);
            }
            else {
                capturedAmount = Mathf.Max(0f, capturedAmount - Globals.goalCaptureAmountAlone * Time.deltaTime);
            }
        }
    }

    private void UpdateFlagPosition() {
        var newPosition = movingFlag.localPosition;
        newPosition.y = capturedAmount * 3.8f;
        movingFlag.localPosition = newPosition;
    }

}
