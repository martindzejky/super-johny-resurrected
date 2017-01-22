using UnityEngine;


/// <summary>
/// Chooses a random sorting layer depth for the mob.
/// </summary>
public class SortMobs : MonoBehaviour {

    public void Awake() {
        var mySprite = GetComponent<SpriteRenderer>();
        var eyesSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        var position = Random.Range(0, (1 << 16) / 2) * 2;

        mySprite.sortingOrder = position;
        eyesSprite.sortingOrder = position + 1;
    }

}
