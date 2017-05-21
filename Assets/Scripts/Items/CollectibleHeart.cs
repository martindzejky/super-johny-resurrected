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
        var col = Physics2D.OverlapBox(transform.position, myCollider.bounds.size, 0,
            LayerMask.GetMask(Globals.mobLayerName));
        if (col) {
            var mob = col.GetComponent<Mob>();
            if (mob) {
                mob.lives++;
                mob.MakeHappy();
                Destroy(gameObject);
            }
        }
    }

}
