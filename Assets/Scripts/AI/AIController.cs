using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls mobs. Attacks enemy mobs, captures goals, and navigates the level.
/// </summary>
public class AIController : MonoBehaviour {

    private Mob myMob;
    private AIState state = AIState.Thinking;
    private Mob closestEnemy = null;
    private Flag closestGoal = null;
    private float retargetTimer;
    private float thinkingTimer;
    private float pathingTimer;
    private Node[] currentPath = null;
    private uint currentPathIndex = 0;
    private float goalCapturePoint;

    public void Awake() {
        myMob = GetComponent<Mob>();
        retargetTimer = Random.Range(0f, Globals.aiRetargetTimer);
        thinkingTimer = Random.Range(0f, Globals.aiThinkTimer);
        pathingTimer = Random.Range(0f, Globals.aiPathingTimer);
        goalCapturePoint = Random.Range(-Globals.aiCaptureRadius, Globals.aiCaptureRadius);
    }

    public void Update() {
        UpdateTimers();

        switch (state) {
            case AIState.Thinking:
                UpdateClosestMob();
                UpdateClosestGoal();
                UpdateStateBasedOnClosestTarget();
                break;

            case AIState.AttackingEnemy:
                AttackEnemy();
                break;

            case AIState.MovingTowardsEnemy:
                MoveTowardsEnemy();
                break;

            case AIState.CapturingGoal:
                CaptureGoal();
                break;

            case AIState.MovingTowardsGoal:
                MoveTowardsGoal();
                break;
        }

        // update closest enemy and goal periodically
        if (retargetTimer < 0f) {
            retargetTimer = Globals.aiRetargetTimer;
            UpdateClosestMob();
            UpdateClosestGoal();
        }

        // update state periodically
        if (thinkingTimer < 0f) {
            thinkingTimer = Globals.aiThinkTimer;
            UpdateStateBasedOnClosestTarget();
        }
    }

    private void UpdateTimers() {
        retargetTimer -= Time.deltaTime;
        thinkingTimer -= Time.deltaTime;
        pathingTimer -= Time.deltaTime;
    }

    private void UpdateClosestMob() {
        closestEnemy = null;

        var numberOfTeams = MobTeams.GetNumberOfTeams();
        var closestDistance = float.PositiveInfinity;

        for (uint i = 0; i < numberOfTeams; i++) {
            if (myMob.team == i && i != 0) {
                continue;
            }

            var team = MobTeams.GetTeam(i);
            if (team.mobs.Count > 0) {
                foreach (var enemy in team.mobs) {
                    var distance = (transform.position - enemy.transform.position).sqrMagnitude;
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
    }

    private void UpdateClosestGoal() {
        closestGoal = null;

        var closestDistance = float.PositiveInfinity;
        var flags = FindObjectsOfType<Flag>();

        foreach (var flag in flags) {
            if (flag.capturedTeam == myMob.team && flag.capturedAmount >= 1f - float.Epsilon) {
                continue;
            }

            var distance = (flag.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance) {
                closestDistance = distance;
                closestGoal = flag;
            }
        }
    }

    private void UpdateStateBasedOnClosestTarget() {
        state = AIState.Wandering;

        if (closestEnemy && closestGoal) {
            if (Vector3.Distance(transform.position, closestEnemy.transform.position) <
                Vector3.Distance(transform.position, closestGoal.transform.position) * Globals.enemyGoalImportanceRatio) {
                UpdateStateBasedOnEnemy();
            }
            else {
                UpdateStateBasedOnGoal();
            }
        }
        else if (closestEnemy) {
            UpdateStateBasedOnEnemy();
        }
        else if (closestGoal) {
            UpdateStateBasedOnGoal();
        }
    }

    private void UpdateStateBasedOnEnemy() {
        var distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);
        if (distanceToEnemy < Globals.aiAttackRadius) {
            var vector = closestEnemy.transform.position - transform.position;
            var wall = Physics2D.Raycast(transform.position, vector.normalized, vector.magnitude, LayerMask.GetMask(Globals.solidLayerName));

            if (wall) {
                state = AIState.MovingTowardsEnemy;
            }
            else {
                state = AIState.AttackingEnemy;
            }
        }
        else {
            state = AIState.MovingTowardsEnemy;
        }
    }

    private void UpdateStateBasedOnGoal() {
        var distanceToGoal = Vector3.Distance(transform.position, closestGoal.transform.position);
        if (distanceToGoal < Globals.aiCaptureRadius) {
            var vector = closestGoal.transform.position - transform.position;
            var wall = Physics2D.Raycast(transform.position, vector.normalized, vector.magnitude, LayerMask.GetMask(Globals.solidLayerName));

            if (wall) {
                state = AIState.MovingTowardsGoal;
            }
            else {
                state = AIState.CapturingGoal;
            }
        }
        else {
            state = AIState.MovingTowardsGoal;
        }
    }

    private void AttackEnemy() {
        if (!closestEnemy) {
            state = AIState.Thinking;
            return;
        }

        UpdateStateBasedOnEnemy();

        var horizontalDistanceToEnemy = closestEnemy.transform.position.x - transform.position.x;
        var directionToEnemy = Mathf.Sign(horizontalDistanceToEnemy);
        var heightDifference = closestEnemy.transform.position.y - transform.position.y;

        if (Mathf.Abs(horizontalDistanceToEnemy) > .8f) {
            myMob.Move(directionToEnemy);
        }

        if (heightDifference > -.5f) {
            myMob.Jump();
        }
    }

    private void CaptureGoal() {
        if (!closestGoal) {
            state = AIState.Thinking;
            return;
        }

        var difference = closestGoal.transform.position.x + goalCapturePoint - transform.position.x;
        if (Mathf.Abs(difference) > 1f) {
            myMob.Move(Mathf.Sign(difference));
        }
    }

    private void MoveTowardsEnemy() {
        if (!closestEnemy) {
            state = AIState.Thinking;
            return;
        }

        UpdateStateBasedOnEnemy();

        // update path periodically
        if (pathingTimer < 0f || currentPath == null || currentPath.Length == 0) {
            pathingTimer = Globals.aiPathingTimer;
            currentPath = PathFinder.GetPath(transform.position, closestEnemy.transform.position);
            currentPathIndex = 0;
        }

        MoveTowardsPathNode();
    }

    private void MoveTowardsGoal() {
        if (!closestGoal) {
            state = AIState.Thinking;
            return;
        }

        UpdateStateBasedOnGoal();

        // update path periodically
        if (pathingTimer < 0f || currentPath == null || currentPath.Length == 0) {
            pathingTimer = Globals.aiPathingTimer;
            currentPath = PathFinder.GetPath(transform.position, closestGoal.transform.position);
            currentPathIndex = 0;
        }

        MoveTowardsPathNode();
    }

    private void MoveTowardsPathNode() {
        if (currentPath.Length == 0) {
            state = AIState.Thinking;
            return;
        }

        var targetNode = currentPath[currentPathIndex];
        var difference = targetNode.transform.position - transform.position;

        // move to the next node if close enough
        if (difference.magnitude < Globals.minNodeContactDistance && currentPathIndex < currentPath.Length - 1) {
            currentPathIndex++;
        }

        myMob.Move(Mathf.Sign(difference.x));
        if (difference.y > .4f) {
            myMob.Jump();
        }
    }

}
