using UnityEngine;


/// <summary>
/// Changes to a top block if no blocks of the same
/// type are above.
/// </summary>
public class TopBlock : MonoBehaviour {

    public GameObject topBlock;

    private void Start() {
        var solid = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 1),
            LayerMask.GetMask(Globals.solidLayerName));

        if (!solid) {
            Instantiate(topBlock, transform.localPosition, transform.localRotation, transform.parent);
            Destroy(gameObject);
        }
    }

}
