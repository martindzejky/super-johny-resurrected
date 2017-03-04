using UnityEngine;
using System.Linq;


/// <summary>
/// Stores info about a player.
/// </summary>
public class PlayerInfo {

    public Mob mob = null;
    public uint team = 0;

    private float respawnTimer = Globals.playerRespawnTime;

    public PlayerInfo(uint team) {
        this.team = team;
    }

    public float GetRespawnTimer() {
        return respawnTimer;
    }

    public bool IsAlive() {
        return mob;
    }

    public void Update() {
        if (!IsAlive()) {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer < 0f && Input.GetButtonDown("Respawn")) {
                respawnTimer = Globals.playerRespawnTime;

                var playerTeam = MobTeams.GetTeam(team);
                if (playerTeam.respawns > 0) {
                    playerTeam.respawns--;

                    var prefabRegistry = GameObject.FindObjectOfType<PrefabRegistry>();
                    var playerFlags = GameObject.FindObjectsOfType<Flag>().Where(flag => flag.IsCapturedByTeam(team)).ToArray();

                    if (playerFlags.Length > 0) {
                        var spawn = playerFlags[Random.Range(0, playerFlags.Length)];
                        mob = spawn.SpawnMob(prefabRegistry.playerMobs[team]).GetComponent<Mob>();
                    }
                }
            }
        }
    }

}
