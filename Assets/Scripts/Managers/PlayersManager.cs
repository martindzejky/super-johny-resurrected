using UnityEngine;


/// <summary>
/// Updates players, respawns them, etc.
/// Also takes care of the local player.
/// </summary>
public class PlayersManager : MonoBehaviour {

    private Mob localPlayer;

    public Mob LocalPlayer {
        get { return localPlayer; }
        set {
            localPlayer = value;
            LocalTeam = value.team;
        }
    }

    public uint LocalTeam { get; set; }

    /// <summary>
    /// Add a player for a random team. Initializes a new playerInfo.
    /// </summary>
    public void CreatePlayerForRandomTeam() {
        var randomTeam = (uint) Random.Range(1, (int) MobTeams.GetNumberOfTeams() + 1);
        CreatePlayer(randomTeam);
    }

    /// <summary>
    /// Add a player to a specific team. Initializes a new playerInfo.
    /// </summary>
    /// <param name="team">The number of the team</param>
    public void CreatePlayer(uint team) {
        var playerInfo = new PlayerInfo(team);
        MobTeams.GetTeam(team).Players.Add(playerInfo);
    }

    public void Update() {
        for (uint i = 0; i < MobTeams.GetNumberOfTeams(); i++) {
            var players = MobTeams.GetTeam(i).Players;
            players.ForEach(player => player.Update());
        }
    }

    public bool IsLocalPlayerAlive() {
        return LocalPlayer;
    }

}
