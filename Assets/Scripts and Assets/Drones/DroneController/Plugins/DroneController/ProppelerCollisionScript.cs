using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DroneController.Physics;

public class ProppelerCollisionScript : MonoBehaviour {

	[SerializeField] private DroneMovement drone;
	private int shortestDistance_index = -1;

	private void Start()
	{
		FindingMyDroneComponent();
	}

	/// <summary>
	/// Will traverse through the parents of this collider and try to find the drone script
	/// so we don't have to do it our selves
	/// </summary>
	void FindingMyDroneComponent()
	{
		Transform currentTransform = transform;
		for (int i = 0; i < 20; i++)
		{
			if (!currentTransform.parent.GetComponent<DroneMovement>())
			{
				currentTransform = currentTransform.parent;
			}
			else
			{
				drone = currentTransform.parent.GetComponent<DroneMovement>();
				FindMyProppeler();
			}
		}
	}

	/// <summary>
	/// Upon successfull drone component found.
	/// Locate closest proppeler to this collider and assign it to this script.
	/// </summary>
	void FindMyProppeler()
	{
		Transform[] proppelers = drone.proppelers;
		float shortestDistance = 999.99f;
		shortestDistance_index = -1;
		for(int i = 0; i < proppelers.Length; i++)
		{
			float currentDistance = Vector3.Distance(transform.position, proppelers[i].position);
			if (currentDistance < shortestDistance)
			{
				shortestDistance = currentDistance;
				shortestDistance_index = i;
			}
		}
	}

	/// <summary>
	/// Called from DroneCollision.cs if proppeler hit an object
	/// </summary>
	public void ProppelerHitObject()
	{
		drone.SlowdownThisProppelerSpeed(transform, shortestDistance_index);
	}

}
