using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Uses nodes and Dijkstra algorithm to navigate the level.
/// </summary>
public class PathFinder {

    /// <summary>
    /// Node pair - current node and the node we got here from.
    /// </summary>
    private struct NodeInfo {
        public Node node;
        public Node parentNode;

        public NodeInfo(Node node, Node parentNode) {
            this.node = node;
            this.parentNode = parentNode;
        }
    }

    public static Node[] GetPath(Vector3 start, Vector3 end) {
        Node closestStart = null;
        Node closestEnd = null;
        float closestStartDistance = float.PositiveInfinity;
        float closestEndDistance = float.PositiveInfinity;

        foreach (var node in GameObject.FindObjectsOfType<Node>()) {
            var distanceStart = (node.transform.position - start).sqrMagnitude;
            var distanceEnd = (node.transform.position - end).sqrMagnitude;

            if (distanceStart < closestStartDistance) {
                closestStartDistance = distanceStart;
                closestStart = node;
            }
            if (distanceEnd < closestEndDistance) {
                closestEndDistance = distanceEnd;
                closestEnd = node;
            }
        }

        return GetPath(closestStart, closestEnd);
    }

    public static Node[] GetPath(Node start, Node end) {
        if (!start || !end) {
            return new Node[] {};
        }

        if (start == end) {
            return new Node[] { start };
        }

        var queue = new Queue<NodeInfo>();
        var visitedNodes = new Dictionary<Node, NodeInfo>();

        queue.Enqueue(new NodeInfo(start, null));

        // go through the whole node graph until we find the end
        while (queue.Count > 0) {
            var nodeInfo = queue.Dequeue();
            visitedNodes[nodeInfo.node] = nodeInfo;

            // if this is the end, reconstruct the path
            if (nodeInfo.node == end) {
                var result = new List<Node>();

                for (var pathNodeInfo = nodeInfo; pathNodeInfo.parentNode; pathNodeInfo = visitedNodes[pathNodeInfo.parentNode]) {
                    result.Add(pathNodeInfo.node);
                }
                result.Add(start);
                result.Reverse();
                return result.ToArray();
            }

            // process connected nodes
            foreach (var connectedNode in nodeInfo.node.connectedNodes) {
                if (visitedNodes.ContainsKey(connectedNode)) {
                    continue;
                }

                queue.Enqueue(new NodeInfo(connectedNode, nodeInfo.node));
            }
        }

        return new Node[] {};
    }

}
