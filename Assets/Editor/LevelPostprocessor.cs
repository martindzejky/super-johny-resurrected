using UnityEngine;
using UnityEditor;
using System.IO;


/// <summary>
/// Used to automatically copy .tmx files to .xml.
/// </summary>
public class LevelPostprocessor : AssetPostprocessor {

    private const string oldExtension = ".tmx";
    private const string newExtension = ".xml";

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths) {
        foreach (var asset in importedAssets) {
            // check extension
            if (!asset.EndsWith(oldExtension, System.StringComparison.OrdinalIgnoreCase)) {
                continue;
            }

            // copy file
            var newPath = asset.Substring(0, asset.Length - oldExtension.Length) + newExtension;
            File.Copy(asset, newPath, true);
            AssetDatabase.Refresh();
            Debug.Log("Updated XML level file: " + asset);
        }
    }

}
