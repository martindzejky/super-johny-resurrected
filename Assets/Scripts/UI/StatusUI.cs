using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Displays the status of every team in the game.
/// </summary>
public class StatusUI : MonoBehaviour {

    public GameObject teamStatusUI;

    private void LateUpdate() {
        // add missing UIs for new teams
        for (var i = transform.childCount; i < MobTeams.GetNumberOfTeams() - 1; i++) {
            var newUI = Instantiate(teamStatusUI);
            newUI.transform.SetParent(transform, false);
            newUI.GetComponent<RectTransform>().offsetMax = new Vector2(0, -10 - 80 * i);
        }

        // get all flags
        var flags = FindObjectsOfType<Flag>();

        // update the UI for each team
        for (var i = 0; i < transform.childCount; i++) {
            var team = (uint) i + 1;
            var teamUI = transform.GetChild(i);
            var mobTeam = MobTeams.GetTeam(team);

            teamUI.Find("MobIcon").GetComponent<Image>().color = mobTeam.teamColor;
            teamUI.Find("FlagIcon").GetComponent<Image>().color = mobTeam.teamColor;

            teamUI.Find("MobCountText").GetComponent<Text>().text = $"{mobTeam.Mobs.Count}";
            teamUI.Find("FlagCountText").GetComponent<Text>().text = $"{flags.Where(flag => flag.IsCapturedByTeam(team)).Count()}";
            teamUI.Find("RespawnText").GetComponent<Text>().text = $"{mobTeam.respawns}";
            teamUI.Find("ScoreText").GetComponent<Text>().text = $"{mobTeam.score}";
        }
    }

}
