using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Uses nodes and Dijkstra algorithm to navigate the level.
/// </summary>
public class PathFinder : MonoBehaviour {

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

    public static List<Node> GetPath(Node start, Node end) {
        var queue = new Queue<NodeInfo>();
        var visitedNodes = new Dictionary<Node, NodeInfo>();
        var result = new List<Node>();

        queue.Enqueue(new NodeInfo(start, null));

        // go through the whole node graph until we find the end
        while (queue.Count > 0) {
            var nodeInfo = queue.Dequeue();
            visitedNodes[nodeInfo.node] = nodeInfo;

            // if this is the end, reconstruct the path
            if (nodeInfo.node == end) {
                for (var pathNodeInfo = nodeInfo; pathNodeInfo.parentNode; pathNodeInfo = visitedNodes[pathNodeInfo.parentNode]) {
                    result.Add(pathNodeInfo.node);
                }
                result.Add(start);

                result.Reverse();
                break;
            }

            // process connected nodes
            foreach (var connectedNode in nodeInfo.node.connectedNodes) {
                if (visitedNodes.ContainsKey(connectedNode)) {
                    continue;
                }

                queue.Enqueue(new NodeInfo(connectedNode, nodeInfo.node));
            }
        }

        return result;
    }

}
