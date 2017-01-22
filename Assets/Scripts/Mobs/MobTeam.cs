using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stores info about a team and a list of mobs.
/// </summary>
public class MobTeam {

    public List<Mob> mobs { get; private set; }
    public Color teamColor = Color.gray;
    public uint respawns = Globals.respawnsPerTeam;
    public uint score = 0;

    public MobTeam(uint teamNumber) {
        this.mobs = new List<Mob>();
    }

}
