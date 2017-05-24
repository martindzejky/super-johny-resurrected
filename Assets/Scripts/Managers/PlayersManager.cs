using UnityEngine;


/// <summary>
/// Updates players, respawns them, etc.
/// Also takes care of the local player.
/// </summary>
public class PlayersManager : MonoBehaviour {

    public PlayerInfo LocalPlayer { get; private set; }

    /// <summary>
    /// Add a player for a random team. Initializes a new playerInfo.
    /// </summary>
    /// <param name="local">Whether this is the local player</param>
    public void CreatePlayerForRandomTeam(bool local = false) {
        var randomTeam = (uint) Random.Range(1, (int) MobTeams.GetNumberOfTeams() + 1);
        CreatePlayer(randomTeam, local);
    }

    /// <summary>
    /// Add a player to a specific team. Initializes a new playerInfo.
    /// </summary>
    /// <param name="team">The number of the team</param>
    /// <param name="local">Whether this is the local player</param>
    public void CreatePlayer(uint team, bool local = false) {
        var playerInfo = new PlayerInfo(team);
        MobTeams.GetTeam(team).Players.Add(playerInfo);

        if (local) {
            LocalPlayer = playerInfo;
        }
    }

    public void Update() {
        for (uint i = 0; i < MobTeams.GetNumberOfTeams(); i++) {
            var players = MobTeams.GetTeam(i).Players;
            players.ForEach(player => player.Update());
        }
    }

}
