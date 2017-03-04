using UnityEngine;


/// <summary>
/// Updates players, respawns them, etc.
/// </summary>
public class PlayersManager : MonoBehaviour {

    public void CreatePlayerForRandomTeam() {
        var randomTeam = (uint) Random.Range(1, (int) MobTeams.GetNumberOfTeams() + 1);
        CreatePlayer(randomTeam);
    }

    public void CreatePlayer(uint team) {
        var playerInfo = new PlayerInfo(team);
        MobTeams.GetTeam(team).players.Add(playerInfo);
    }

    public void Update() {
        for (uint i = 0; i < MobTeams.GetNumberOfTeams(); i++) {
            var players = MobTeams.GetTeam(i).players;
            players.ForEach(player => player.Update());
        }
    }

}
