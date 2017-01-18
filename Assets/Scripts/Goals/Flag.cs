using UnityEngine;


/// <summary>
/// Flag is a goal that can be captured by mobs.
/// </summary>
public class Flag : MonoBehaviour {

    public Transform movingFlag;

    private Collider2D myCollider;
    private uint capturedTeam = 0;
    private float capturedAmount = 0f;

    public void Awake() {
        myCollider = GetComponent<Collider2D>();
    }

    public void Update() {
        var mobs = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f, LayerMask.GetMask(Globals.mobLayerName));

        // let mobs capture the flag
        foreach (var mobObject in mobs) {
            var mob = mobObject.GetComponent<Mob>();

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

        // update flag position
        var newPosition = movingFlag.localPosition;
        newPosition.y = capturedAmount * 3.8f;
        movingFlag.localPosition = newPosition;
    }

}
