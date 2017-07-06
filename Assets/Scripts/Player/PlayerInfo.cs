using System.Linq;
using UnityEngine;


/// <summary>
/// Stores info about a player.
/// </summary>
public class PlayerInfo {

    /// <summary>The mob of the player. If null, the player is currently dead.</summary>
    public Mob mob;

    public uint team;

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
        // respawn the player if he is not alive, the player must press the respawn key
        if (IsAlive()) {
            return;
        }

        respawnTimer -= Time.deltaTime;

        var playerTeam = MobTeams.GetTeam(team);
        if (playerTeam.respawns <= 0) {
            return;
        }

        var prefabRegistry = Object.FindObjectOfType<PrefabRegistry>();
        var playerFlags = Object.FindObjectsOfType<Flag>()
            .Where(flag => flag.IsCapturedByTeam(team))
            .ToArray();

        if (respawnTimer < 0f && Input.GetButtonDown("Respawn") && playerFlags.Length > 0) {
            playerTeam.respawns--;
            respawnTimer = Globals.playerRespawnTime;

            var spawn = playerFlags[Random.Range(0, playerFlags.Length)];
            mob = spawn.SpawnMob(prefabRegistry.playerMobs[team]).GetComponent<Mob>();
        }
    }

}
