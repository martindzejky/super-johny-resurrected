using UnityEngine;


/// <summary>
/// Spawns a flag captured by a team.
/// </summary>
public class CapturedFlag : MonoBehaviour {

    public uint capturedTeam = 0;
    public bool locked = false;

    void Start () {
        var registry = FindObjectOfType<PrefabRegistry>();
        var flag = Instantiate(registry.goal, transform.position, transform.rotation).GetComponent<Flag>();
        flag.capturedAmount = 1f;
        flag.capturedTeam = capturedTeam;
        flag.locked = locked;
        Destroy(gameObject);
    }

}
