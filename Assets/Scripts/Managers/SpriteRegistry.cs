using AdvancedInspector;
using UnityEngine;


/// <summary>
/// Contains lists for all sprites. Used mostly for selecting random sprites.
/// </summary>
[AdvancedInspector(true, false)]
public class SpriteRegistry : MonoBehaviour {

    [Help(HelpType.Info, "One of the bodies is selected when spawning a new mob")]
    [Collection(Sortable = false, AlwaysExpanded = true)]
    public Sprite[] mobBodies;

}
