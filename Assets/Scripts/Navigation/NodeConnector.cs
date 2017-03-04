using UnityEngine;


/// <summary>
/// Connects nodes in one direction.
/// </summary>
public class NodeConnector : MonoBehaviour {

    public void Start() {
        var nodes = FindObjectsOfType<Node>();

        // find neighbor nodes
        foreach (var node in nodes) {
            var dirFromNode = transform.position - node.transform.position;
            var dirFromNodeAngle = Mathf.Atan2(dirFromNode.y, dirFromNode.x);
            if (dirFromNode.magnitude > 1.5f) {
                continue;
            }

            // we have a neighbor, find the closest node in that direction
            Node closestNode = null;
            var closestDistance = float.PositiveInfinity;
            foreach (var targetNode in nodes) {
                if (targetNode == node) {
                    continue;
                }

                var dirToTarget = targetNode.transform.position - node.transform.position;
                var dirToTargetAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x);

                if (Mathf.Abs(dirFromNodeAngle - dirToTargetAngle) < .1f) {
                    var distance = dirToTarget.sqrMagnitude;
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestNode = targetNode;
                    }
                }
            }

            // connect the node
            node.connectedNodes.Add(closestNode);
        }
    }

}
