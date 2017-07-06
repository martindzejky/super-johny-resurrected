using UnityEngine;
using UnityEngine.UI;


public class RespawnUI : MonoBehaviour {

    public Image backdrop;
    public Text respawnText;

    private PlayersManager playersManager;

    private void Awake() {
        playersManager = FindObjectOfType<PlayersManager>();
    }

    private void LateUpdate() {
        if (playersManager.LocalPlayer.IsAlive()) {
            backdrop.enabled = false;
            respawnText.enabled = false;
        }
        else {
            backdrop.enabled = true;
            respawnText.enabled = true;

            var respawnTime = playersManager.LocalPlayer.GetRespawnTimer();
            var team = MobTeams.GetTeam(playersManager.LocalPlayer.team);

            if (team.respawns <= 0) {
                respawnText.text = "Your team is out of respawn points";
            }
            else if (respawnTime < Globals.playerRespawnTime - 2f && respawnTime > 0f) {
                respawnText.text = $"You can spawn in {Mathf.Ceil(respawnTime)} seconds";
            }
            else if (respawnTime <= 0f) {
                respawnText.text = "Spawn now using the 'Respawn' key";
            }
            else {
                respawnText.enabled = false;
            }
        }
    }


}
