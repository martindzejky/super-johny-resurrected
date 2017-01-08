using UnityEngine;


/// <summary>
/// Makes the mob constantly jump.
/// </summary>
public class JumpingDummy : MonoBehaviour {

    private Mob mob;

    public void Awake() {
        mob = GetComponent<Mob>();
    }

    public void Update() {
        mob.Jump();
    }

}
