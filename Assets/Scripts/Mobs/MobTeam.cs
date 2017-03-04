using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Stores info about a team and a list of mobs.
/// </summary>
public class MobTeam {

    public Color teamColor = Color.white;
    public uint respawns = Globals.respawnsPerTeam;
    public uint score = 0;

    public List<Mob> mobs { get; private set; }
    public List<PlayerInfo> players { get; private set; }

    public MobTeam(uint teamNumber) {
        this.mobs = new List<Mob>();
        this.players = new List<PlayerInfo>();
    }

    public bool HasPlayers() {
        return players.Count > 0;
    }

    public bool HasAlivePlayers() {
        return players.Any(player => player.IsAlive());
    }

}
