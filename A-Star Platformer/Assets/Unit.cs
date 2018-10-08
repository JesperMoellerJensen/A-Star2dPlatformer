using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	public Transform target;
	float speed = 10f;
	Vector2[] path;
	int targetIndex;

	bool FindingPath = false;

	private void Start()
	{
		InvokeRepeating("FindPlayer", 0f, 0.01f);
	}

	public void FindPlayer()
	{
		if (FindingPath == false)
		{
			FindingPath = true;
			PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		}

	}


	public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
		FindingPath = false;
	}

	IEnumerator FollowPath()
	{
		Vector2 currentWaypoint = path[0];

		while (true)
		{
			if (new Vector2(transform.position.x, transform.position.y) == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					yield break;
				}

				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one / 5);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}
