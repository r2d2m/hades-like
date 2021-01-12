using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : Enemy {

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .2f;

    public Transform target; // Initiate to player.
    public float turnSpeed = 2;
    public float turnDst = 1;

    int pathIndex;
    public bool followingPath;

    public Path path;


    // Sets new path if available.
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            path = new Path(waypoints, transform.position, turnDst);
            pathIndex = 0;
            followingPath = true;
        }
    }


    // Updates path if enemy moves.
    public IEnumerator UpdatePath() {

        if (Time.timeSinceLevelLoad < .3f) {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true) {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    // Returns movement direction along path.
    public Vector3 GetPathVector(Vector3 currentPosition) {
        //TODO pathIndex out of bouds buG!?=?!=!?==!=!
        if (path != null && pathIndex < path.lookPoints.Length) {
            Vector3 forwardDirection = (path.lookPoints[pathIndex] - currentPosition).normalized;
            Vector2 pos2D = new Vector2(currentPosition.x, currentPosition.y);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
                if (pathIndex == path.finishLineIndex) {
                    followingPath = false;
                    break;
                } else {
                    pathIndex++;
                }
            }

            Vector3 targetDirection = (path.lookPoints[pathIndex] - currentPosition).normalized;
            forwardDirection = Vector3.RotateTowards(forwardDirection, targetDirection, Time.deltaTime * turnSpeed, 0.0f);

            return forwardDirection;
        } else {
            return new Vector3(0, 0, 0);
        }
    }


    // Draws path of object.
    public void OnDrawGizmos() {
        if (path != null) {
            path.DrawWithGizmos();
        }
    }


}
