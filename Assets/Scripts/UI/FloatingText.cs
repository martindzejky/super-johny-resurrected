using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Displays a small text floating up. The text is destroyed after the specified time.
/// </summary>
public class FloatingText : MonoBehaviour {

    private float timer = Globals.floatingTextTime;

    private const float speed = .5f;
    private const float defaultScale = .004f;

    /// <summary>
    /// Set the text of this floating text.
    /// </summary>
    /// <param name="text">The text to display</param>
    /// <param name="scale">If true, assumes that the text is a number and scales the text according to the number</param>
    public void SetText(string text, bool scale = true) {
        var textChild = transform.GetChild(0);
        textChild.GetComponent<Text>().text = text;

        if (scale) {
            int score;
            if (int.TryParse(text, out score)) {
                var newScale = defaultScale + .002f * (score / 10f - 1f);
                textChild.localScale = new Vector3(newScale, newScale, 1f);
            }
        }
    }

    public void Update() {
        transform.Translate(0f, speed * Time.deltaTime, 0f);
        timer -= Time.deltaTime;
        if (timer < 0f) {
            Destroy(gameObject);
        }
    }

}
