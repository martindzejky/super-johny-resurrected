using System.Collections.Generic;


/// <summary>
/// Stores info about a team and a list of mobs.
/// </summary>
public class MobTeam {

    public List<Mob> mobs { get; private set; }

    public MobTeam(uint teamNumber) {
        this.mobs = new List<Mob>();
    }

}
