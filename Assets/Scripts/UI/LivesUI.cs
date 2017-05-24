using UnityEngine;
using UnityEngine.UI;


public class LivesUI : MonoBehaviour {

    public Image heartImage;
    public Text livesText;

    private PlayerController player;

    private void LateUpdate() {
        if (!player) {
            player = FindObjectOfType<PlayerController>();

            if (!player) {
                heartImage.enabled = false;
                livesText.enabled = false;
                return;
            }

            heartImage.enabled = true;
            livesText.enabled = true;
        }

        livesText.text = player.GetComponent<Mob>().lives.ToString();
    }

}
