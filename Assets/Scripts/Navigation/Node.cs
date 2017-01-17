using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Connects to other nodes when initializing. It is used for navigation.
/// </summary>
public class Node : MonoBehaviour {

    public List<Node> connectedNodes { get; private set; }

    public void Awake() {
        connectedNodes = new List<Node>();
    }

    public void Start() {
        // connect to all nodes that are close enough and visible by this one
        var nodes = FindObjectsOfType<Node>();
        foreach (var otherNode in nodes) {
            if (otherNode == this) {
                continue;
            }

            var vector = otherNode.transform.position - transform.position;
            var distance = vector.magnitude;
            if (distance > Globals.maxNodeDistance) {
                continue;
            }

            var direction = vector.normalized;
            var obstacle = Physics2D.Raycast(transform.position + direction, direction, distance - 1f,
                LayerMask.GetMask(new string[]{ Globals.navigationLayerName, Globals.solidLayerName }));

            if (!obstacle || obstacle.collider.GetComponent<Node>() == otherNode) {
                connectedNodes.Add(otherNode);
            }
        }
    }

}
