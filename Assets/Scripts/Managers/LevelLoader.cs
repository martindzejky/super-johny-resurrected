using UnityEngine;
using System.Xml;
using System.Collections.Generic;


/// <summary>
/// Loads an XML level file and instantiates the prefabs.
/// </summary>
public class LevelLoader : MonoBehaviour {

    public Transform container;

    private Dictionary<int, GameObject> tiles;

    public void Awake() {
        MobTeams.Reset();
        LoadLevel("MultiLayerArea");
        CreatePlayer();
    }

    /// <summary>
    /// Load a level by the given name. Create all the tiles and put them in the container game object.
    /// </summary>
    /// <param name="levelName">Name of the level to load</param>
    private void LoadLevel(string levelName) {
        tiles = new Dictionary<int, GameObject>();
        var prefabRegistry = FindObjectOfType<PrefabRegistry>();

        Debug.Log("Parsing level " + levelName);

        var levelText = Resources.Load<TextAsset>("Levels/" + levelName);
        var level = new XmlDocument();
        level.LoadXml(levelText.ToString());

        // parse tilesets
        var tilesets = level.SelectNodes("//tileset");
        foreach (XmlNode tileset in tilesets) {
            var idOffsetString = tileset.Attributes["firstgid"].Value;
            var idOffset = int.Parse(idOffsetString);

            foreach (XmlNode tile in tileset.ChildNodes) {
                var idString = tile.Attributes["id"].Value;
                var id = int.Parse(idString) + idOffset;

                // find the matching game object
                var name = tile.FirstChild.Attributes["source"].Value;
                for (var i = 0; i < prefabRegistry.tileIds.Length; i++) {
                    if (name.Contains(prefabRegistry.tileIds[i])) {
                        // store in the dictionary
                        tiles[id] = prefabRegistry.tiles[i];
                        break;
                    }
                }
            }
        }

        Debug.Log("Parsed " + tiles.Count + " tiles");

        // parse layers and create tiles
        var layers = level.SelectNodes("//layer");
        foreach (XmlNode layer in layers) {
            Debug.Log("Parsing layer " + layer.Attributes["name"].Value);

            var data = layer.FirstChild.InnerText.Trim();
            var rows = data.Split('\n');

            for (var y = 0; y < rows.Length; y++) {
                var cells = rows[y].Split(',');
                for (var x = 0; x < cells.Length; x++) {
                    if (cells[x] == "") {
                        continue;
                    }

                    var tileId = int.Parse(cells[x]);

                    if (tileId <= 0) {
                        continue;
                    }

                    // instantiate the tile
                    if (tiles.ContainsKey(tileId)) {
                        var tile = Instantiate(tiles[tileId]);
                        tile.transform.SetParent(container);
                        tile.transform.position = new Vector3(x, -y, 0);
                    }
                }
            }
        }
    }

    private static void CreatePlayer() {
        var playersManager = FindObjectOfType<PlayersManager>();
        playersManager.CreatePlayerForRandomTeam(true);
    }

}
