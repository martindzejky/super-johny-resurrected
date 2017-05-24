using UnityEngine;


/// <summary>
/// Makes the camera follow the player if they are alive.
/// If they are dead, the player can choose a friendly mob to follow.
/// </summary>
public class CameraFollowing : MonoBehaviour {

    private PlayersManager playersManager;
    private Mob followedMob;
    private float deadTimer = Globals.cameraAfterDeathTimeout;

    private void Awake() {
        playersManager = FindObjectOfType<PlayersManager>();
    }

    public void LateUpdate() {
        if (playersManager.IsLocalPlayerAlive()) {
            deadTimer = Globals.cameraAfterDeathTimeout;
            FollowMob(playersManager.LocalPlayer);
        }
        else {
            HandleDeadPlayer();
        }
    }

    private void FollowMob(Mob mob) {
        transform.position = new Vector3(mob.transform.position.x, mob.transform.position.y, -10f);
    }

    private void HandleDeadPlayer() {
        deadTimer -= Time.deltaTime;

        if (deadTimer <= 0) {
            if (followedMob) {
                FollowMob(followedMob);
            }
            else {
                // reset death timer
                deadTimer = Globals.cameraAfterDeathTimeout;
            }
        }
        else if (!followedMob) {
            // quickly find another mob
            var mobs = MobTeams.GetTeam(playersManager.LocalTeam).Mobs;
            if (mobs.Count > 0) {
                followedMob = mobs[Random.Range(0, mobs.Count)];
            }
        }
    }

}
