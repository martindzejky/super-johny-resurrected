using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls mobs. Attacks enemy mobs, captures goals, and navigates the level.
/// </summary>
public class AIController : MonoBehaviour {

    private Mob myMob;
    private AIState state = AIState.Thinking;
    private Mob closestEnemy = null;
    private float retargetTimer;
    private float thinkingTimer;
    private float pathingTimer;
    private Node[] currentPath = null;
    private uint currentPathIndex = 0;

    public void Awake() {
        myMob = GetComponent<Mob>();
        retargetTimer = Random.Range(0f, Globals.aiRetargetTimer);
        thinkingTimer = Random.Range(0f, Globals.aiThinkTimer);
        pathingTimer = Random.Range(0f, Globals.aiPathingTimer);
    }

    public void Update() {
        UpdateTimers();

        switch (state) {
            case AIState.Thinking:
                UpdateClosestMob();
                UpdateStateBasedOnClosestTarget();
                break;

            case AIState.AttackingEnemy:
                AttackEnemy();
                break;

            case AIState.MovingTowardsEnemy:
                MoveTowardsEnemy();
                break;
        }

        // update closest enemy periodically
        if (retargetTimer < 0f) {
            retargetTimer = Globals.aiRetargetTimer;
            UpdateClosestMob();
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
        var closestDistance = float.MaxValue;

        for (uint i = 0; i < numberOfTeams; i++) {
            if (myMob.team == i && i != 0) {
                continue;
            }

            var team = MobTeams.GetTeam(i);
            if (team.mobs.Count > 0) {
                foreach (var enemy in team.mobs) {
                    var distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
    }

    private void UpdateStateBasedOnClosestTarget() {
        if (thinkingTimer < 0f) {
            thinkingTimer = Globals.aiThinkTimer;
        }
        else {
            return;
        }

        state = AIState.Wandering;

        // check the enemies and attack / chase them if close enough
        if (closestEnemy) {
            UpdateStateBasedOnEnemy();
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
