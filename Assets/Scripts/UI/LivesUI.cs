using UnityEngine;
using UnityEngine.UI;


public class LivesUI : MonoBehaviour {

    public Image heartImage;
    public Text livesText;

    private PlayersManager playersManager;

    private void Awake() {
        playersManager = FindObjectOfType<PlayersManager>();
    }

    private void LateUpdate() {
        if (playersManager.LocalPlayer.IsAlive()) {
            heartImage.enabled = true;
            livesText.enabled = true;
            livesText.text = playersManager.LocalPlayer.mob.lives.ToString();
        }
        else {
            heartImage.enabled = false;
            livesText.enabled = false;
        }
    }

}
