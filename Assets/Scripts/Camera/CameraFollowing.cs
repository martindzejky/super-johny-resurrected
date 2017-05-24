using UnityEngine;


/// <summary>
/// Makes the camera follow the player if they are alive.
/// If they are dead, the player can choose a friendly mob to follow.
/// </summary>
public class CameraFollowing : MonoBehaviour {

    private PlayersManager playersManager;
    private Mob followedMob;
    private Flag fallbackFlag;
    private float deadTimer = Globals.cameraAfterDeathTimeout;
    private bool initialPosition;

    private void Awake() {
        playersManager = FindObjectOfType<PlayersManager>();

        var flags = FindObjectsOfType<Flag>();
        if (flags.Length > 0) {
            fallbackFlag = flags[Random.Range(0, flags.Length)];
        }
    }

    public void LateUpdate() {
        if (playersManager.LocalPlayer.IsAlive()) {
            deadTimer = Globals.cameraAfterDeathTimeout;
            FollowObject(playersManager.LocalPlayer.mob);
        }
        else {
            HandleDeadPlayer();
        }
    }

    private void FollowObject(Component @object) {
        transform.position = new Vector3(@object.transform.position.x, @object.transform.position.y, -10f);
    }

    private void HandleDeadPlayer() {
        deadTimer -= Time.deltaTime;

        // set initial position
        if (!initialPosition && fallbackFlag) {
            initialPosition = true;
            FollowObject(fallbackFlag);
        }

        if (deadTimer <= 0) {
            if (followedMob) {
                FollowObject(followedMob);
            }
            else {
                // reset death timer
                deadTimer = Globals.cameraAfterDeathTimeout;
            }
        }
        else if (!followedMob) {
            // quickly find another mob
            var mobs = MobTeams.GetTeam(playersManager.LocalPlayer.team).Mobs;
            if (mobs.Count > 0) {
                followedMob = mobs[Random.Range(0, mobs.Count)];
            }

            // fallback
            else if (fallbackFlag) {
                FollowObject(fallbackFlag);
            }
        }
    }

}
