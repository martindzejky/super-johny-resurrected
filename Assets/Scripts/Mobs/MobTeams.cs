using System.Collections.Generic;


/// <summary>
/// Stores mob teams.
/// </summary>
public class MobTeams {

    private static readonly Dictionary<uint, MobTeam> teams = new Dictionary<uint, MobTeam>();

    /// <summary>
    /// Remove all teams.
    /// </summary>
    public static void Reset() {
        teams.Clear();
    }

    /// <summary>
    /// Get a team with a team number. If the team does not exist yet, it is created first.
    /// </summary>
    /// <param name="teamNumber">The number of the team</param>
    /// <returns>The team</returns>
    public static MobTeam GetTeam(uint teamNumber) {
        if (teams.ContainsKey(teamNumber)) {
            return teams[teamNumber];
        }

        var mobTeam = new MobTeam();
        teams[teamNumber] = mobTeam;
        return mobTeam;
    }

    public static uint GetNumberOfTeams() {
        return (uint) teams.Count;
    }

}
