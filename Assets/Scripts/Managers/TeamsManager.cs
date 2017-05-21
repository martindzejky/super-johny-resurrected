using System.Linq;
using UnityEngine;


/// <summary>
/// Respawns team mobs based on the number of captured goals and alive mobs.
/// </summary>
public class TeamsManager : MonoBehaviour {

    private void Update() {
        for (uint i = 0; i < MobTeams.GetNumberOfTeams(); i++) {
            var team = MobTeams.GetTeam(i);

            // decrease the timer
            team.respawnTimer -= Time.deltaTime;

            // respawn if zero
            if (team.respawnTimer <= 0 && team.respawns > 0) {
                var prefabRegistry = FindObjectOfType<PrefabRegistry>();
                var capturedFlags = FindObjectsOfType<Flag>()
                    .Where(flag => flag.IsCapturedByTeam(i))
                    .ToArray();

                if (capturedFlags.Length > 0 && prefabRegistry.teamMobs[i]) {
                    var spawn = capturedFlags[Random.Range(0, capturedFlags.Length)];
                    spawn.SpawnMob(prefabRegistry.teamMobs[i]).GetComponent<Mob>();

                    team.respawnTimer = RespawnTimerForTeam(team.Mobs.Count, capturedFlags.Length);
                    team.respawns--;
                }
            }
        }
    }

    private static float RespawnTimerForTeam(int mobCount, int capturedFlagsCount) {
        return Mathf.Max(Globals.minimumRespawnTime,
            mobCount * Globals.respawnTimePerAliveMob + capturedFlagsCount * Globals.respawnTimePerCapturedFlag);
    }

}
