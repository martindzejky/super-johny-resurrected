using System;
using AdvancedInspector;
using UnityEngine;


/// <summary>
/// Contains lists for all prefabs. One-stop place for all instantiable prefabs in the game.
/// </summary>
[AdvancedInspector(true, false)]
public class PrefabRegistry : MonoBehaviour {

    /// <summary>
    /// Used to make an editable dictionary for the tiles.
    /// </summary>
    [Serializable]
    public class TilePrefabsDictionary : UDictionary<string, GameObject> {}

    [Help(HelpType.Info, "Tiles are used when a level is loaded")]
    public TilePrefabsDictionary tiles = new TilePrefabsDictionary();

    [Help(HelpType.Warning, "The index in the array denotes the team number")]
    public GameObject[] teamMobs;

    [Help(HelpType.Warning, "The index in the array denotes the team number")]
    public GameObject[] playerMobs;

    [Group("Miscellaneous")]
    public GameObject deadBody;

    [Group("Miscellaneous")]
    public GameObject lostHeart;

    [Group("Miscellaneous")]
    public GameObject starParticle;

    [Group("Miscellaneous")]
    public GameObject floatingText;

    [Group("Miscellaneous")]
    public GameObject goal;

}