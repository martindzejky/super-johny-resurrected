using UnityEngine;


/// <summary>
/// Adds lives when collected.
/// </summary>
public class CollectibleHeart : MonoBehaviour {

    private Collider2D myCollider;

    public void Awake() {
        myCollider = GetComponent<Collider2D>();
    }

    public void Update() {
        var collider = Physics2D.OverlapBox(transform.position, myCollider.bounds.size, 0, LayerMask.GetMask(Globals.mobLayerName));
        if (collider) {
            var mob = collider.GetComponent<Mob>();
            if (mob) {
                mob.lives++;
                Destroy(gameObject);
            }
        }
    }

}
