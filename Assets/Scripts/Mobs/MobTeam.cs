using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Stores info about a team and a list of mobs.
/// </summary>
public class MobTeam {

    public Color teamColor = Color.white;
    public uint respawns = Globals.respawnsPerTeam;
    public float respawnTimer = Globals.startingRespawnTime;
    public uint score = 0;

    public List<Mob> Mobs { get; }
    public List<PlayerInfo> Players { get; }

    public MobTeam() {
        Mobs = new List<Mob>();
        Players = new List<PlayerInfo>();
    }

    public bool HasPlayers() {
        return Players.Count > 0;
    }

    public bool HasAlivePlayers() {
        return Players.Any(player => player.IsAlive());
    }

}
