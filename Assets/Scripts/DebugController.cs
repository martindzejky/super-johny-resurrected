using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


/**
 * Allows the player to quickly restart and quit the game.
 */
public class DebugController : MonoBehaviour {

    public void Update() {
        if (Input.GetButtonDown("Restart")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetButtonDown("Cancel")) {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

}
