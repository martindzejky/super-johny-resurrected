using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


/// <summary>
/// Allows the player to quickly restart and quit the game.
/// </summary>
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
