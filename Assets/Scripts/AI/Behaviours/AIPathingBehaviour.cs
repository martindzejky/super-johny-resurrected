using UnityEngine;


/// <summary>
/// Defines a generic behaviour that supports pathing and navigation.
/// </summary>
public class AIPathingBehaviour : AIBehaviour {

    protected Node[] currentPath = null;
    protected uint currentPathIndex = 0;

    public AIPathingBehaviour(AIController controller, Mob mob) : base(controller, mob) {}

    protected void NavigateTowards(Vector3 destination) {
        currentPath = PathFinder.GetPath(controller.transform.position, destination);
        currentPathIndex = 0;
    }

    protected void MoveTowardsPathNode() {
        if (currentPath.Length == 0) {
            return;
        }

        var targetNode = currentPath[currentPathIndex];
        var difference = targetNode.transform.position - controller.transform.position;

        // move to the next node if close enough
        if (difference.magnitude < Globals.minNodeContactDistance && currentPathIndex < currentPath.Length - 1) {
            currentPathIndex++;
        }

        mob.Move(Mathf.Sign(difference.x));
        if (difference.y > .4f) {
            mob.Jump();
        }
    }

}
