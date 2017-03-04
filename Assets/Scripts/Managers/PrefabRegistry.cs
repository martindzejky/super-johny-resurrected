using UnityEngine;


/// <summary>
/// Contains lists for all prefabs. One-stop place for all instantiable prefabs in the game.
/// </summary>
public class PrefabRegistry : MonoBehaviour {

    public string[] tileIds;
    public GameObject[] tiles;

    public GameObject[] teamMobs;
    public GameObject[] playerMobs;

    public GameObject deadBody;
    public GameObject lostHeart;
    public GameObject starParticle;
    public GameObject floatingText;
    public GameObject goal;

}
