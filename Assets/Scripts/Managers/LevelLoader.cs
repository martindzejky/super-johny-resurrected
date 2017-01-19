using UnityEngine;


/// <summary>
/// Loads an XML level file and instantiates the prefabs.
/// </summary>
public class LevelLoader : MonoBehaviour {

    public string levelName = "TestArea";
    public Transform container;

    public void Awake() {
        var prefabRegistry = FindObjectOfType<PrefabRegistry>();
        var prefabs = prefabRegistry.tiles;

        // load the level from the Resources directory
        var levelText = Resources.Load<TextAsset>("Levels/" + levelName);
        var levelRows = levelText.text.Split('\n');

        for (var y = 0; y < levelRows.Length - 1; y++) {
            var levelRow = levelRows[y].Split(',');

            for (var x = 0; x < levelRow.Length; x++) {
                // get tile index
                var tileIndex = int.Parse(levelRow[x]);

                if (tileIndex < 0) {
                    continue;
                }

                // instantiate and set up the tile
                // mirror the tile position's Y coordinate
                if (prefabs[tileIndex]) {
                    var tile = Instantiate(prefabs[tileIndex]);
                    tile.transform.SetParent(container);
                    tile.transform.position = new Vector3(x, -y, 0);
                }
            }
        }
    }

}
