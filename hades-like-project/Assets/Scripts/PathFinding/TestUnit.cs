using UnityEngine;
using System.Collections;

public class TestUnit : MonoBehaviour {

    const float minPathUpdateTime = .2f;
	const float pathUpdateMoveThreshold = .2f;

	public Transform target;
	public float speed = 4;
    public float turnSpeed = 1;
    public float turnDst = 2;
	
    Path path;

	void Start() {
		StartCoroutine (UpdatePath ());
	}

	public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			path = new Path(waypoints,transform.position,turnDst);
			
            StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

    IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		Vector3 targetPosOld = target.position;

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
				targetPosOld = target.position;
			}
		}
	}

    

	IEnumerator FollowPath() {
		bool followingPath = true;
		int pathIndex = 0;

		Vector3 forwardDirection = (path.lookPoints[0]-transform.position).normalized;
		Debug.Log(forwardDirection);


		while (true) {
			Vector2 pos2D = new Vector2(transform.position.x,transform.position.y);
			while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)){
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex ++;
				}
			}

			if (followingPath) {
				Vector3 targetDirection = (path.lookPoints[pathIndex] - transform.position).normalized;
				forwardDirection = Vector3.RotateTowards(forwardDirection,targetDirection,Time.deltaTime * turnSpeed,0.0f);
				transform.Translate (forwardDirection * Time.deltaTime * speed, Space.World);
			}

			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos ();
		}
	}
}