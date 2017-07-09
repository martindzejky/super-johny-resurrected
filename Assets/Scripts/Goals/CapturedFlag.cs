using UnityEngine;


/// <summary>
/// Spawns a flag captured by a team.
/// </summary>
public class CapturedFlag : MonoBehaviour {

    public uint capturedTeam;
    public bool locked;

    public void Start () {
        var registry = FindObjectOfType<PrefabRegistry>();
        var flag = Instantiate(registry.goal, transform.position, transform.rotation).GetComponent<Flag>();
        flag.Capture(capturedTeam);
        flag.locked = locked;
        Destroy(gameObject);
    }

}
