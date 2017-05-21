using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Connects to other nodes when initializing. It is used for navigation.
/// </summary>
public class Node : MonoBehaviour {

    public List<Node> ConnectedNodes { get; private set; }

    private SpriteRenderer spriteRenderer;

    public void Awake() {
        ConnectedNodes = new List<Node>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = Globals.debugging;
    }

    public void Start() {
        // connect to all nodes that are close enough and visible by this one
        // stopped by solids and other navigation nodes
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
                LayerMask.GetMask(Globals.navigationLayerName, Globals.solidLayerName));

            if (!obstacle || obstacle.collider.GetComponent<Node>() == otherNode) {
                ConnectedNodes.Add(otherNode);
            }
        }
    }

    public void Update() {
        if (spriteRenderer.enabled) {
            foreach (var node in ConnectedNodes) {
                Debug.DrawRay(transform.position, (node.transform.position - transform.position) / 2f, Color.red);
            }
        }
    }

}
