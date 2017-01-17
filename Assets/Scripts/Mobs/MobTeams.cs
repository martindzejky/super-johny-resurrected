using System.Collections.Generic;


/// <summary>
/// Stores mob teams.
/// </summary>
public class MobTeams {

    private static Dictionary<uint, MobTeam> teams = new Dictionary<uint, MobTeam>();

    public static MobTeam GetTeam(uint teamNumber) {
        if (teams.ContainsKey(teamNumber)) {
            return teams[teamNumber];
        }
        else {
            var mobTeam = new MobTeam(teamNumber);
            teams[teamNumber] = mobTeam;
            return mobTeam;
        }
    }

    public static uint GetNumberOfTeams() {
        return (uint) teams.Count;
    }

}
