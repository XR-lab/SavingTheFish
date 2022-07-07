using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
	[SerializeField] private float FOVAngle;
	[SerializeField] private float smoothDamp;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private Vector3[] directionsToCheckWhenAvoidingObstacles;

	private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
	private List<FlockUnit> avoidanceNeighbours = new List<FlockUnit>();
	private List<FlockUnit> aligementNeighbours = new List<FlockUnit>();
	private Flock assignedFlock;
	private Vector3 currentVelocity;
	private Vector3 currentObstacleAvoidanceVector;
	private float speed;
	private float rangeMin = 0;
	private float rangeMax = 1;

	public bool canMove = true;

	public Transform myTransform { get; set; }

	private void Awake()
	{
		myTransform = transform;
	}

    public void AssignFlock(Flock flock)
	{
		assignedFlock = flock;
	}

	public void InitializeSpeed(float speed)
	{
		this.speed = speed;
	}

	public void MoveUnit()
	{
		if (myTransform == null || !canMove) return;
		FindNeighbours();
		CalculateSpeed();

		var cohesionVector = CalculateCohesionVector() * assignedFlock.cohesionWeight;
		var avoidanceVector = CalculateAvoidanceVector() * assignedFlock.avoidanceWeight;
		var aligementVector = CalculateAligementVector() * assignedFlock.aligementWeight;
		var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;
		var obstacleVector = CalculateObstacleVector() * assignedFlock.obstacleWeight;

		var moveVector = cohesionVector + avoidanceVector + aligementVector + boundsVector + obstacleVector;
		moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
		moveVector = moveVector.normalized * speed;
		if (moveVector == Vector3.zero)
			moveVector = transform.forward;

		myTransform.forward = moveVector;
		myTransform.position += moveVector * Time.deltaTime;
	}



	private void FindNeighbours()
	{
		cohesionNeighbours.Clear();
		avoidanceNeighbours.Clear();
		aligementNeighbours.Clear();
		var allUnits = assignedFlock.allUnits;
		for (int i = 0; i < allUnits.Length; i++)
		{
			var currentUnit = allUnits[i];
			if (currentUnit == null) continue;
			//if (currentUnit.myTransform == null) continue;
			if (myTransform == null) continue;
			float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.myTransform.position - myTransform.position);
				if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
				{
					cohesionNeighbours.Add(currentUnit);
				}
				if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
				{
					avoidanceNeighbours.Add(currentUnit);
				}
				if (currentNeighbourDistanceSqr <= assignedFlock.aligementDistance * assignedFlock.aligementDistance)
				{
					aligementNeighbours.Add(currentUnit);
				}
			
		}
	}

	private void CalculateSpeed()
	{
		if (cohesionNeighbours.Count == 0)
			return;
		speed = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			speed += cohesionNeighbours[i].speed;
		}

		speed /= cohesionNeighbours.Count;
		speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
	}

	private Vector3 CalculateCohesionVector()
	{
		var cohesionVector = Vector3.zero;
		if (cohesionNeighbours.Count == 0)
			return Vector3.zero;
		int neighboursInFOV = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			if (IsInFOV(cohesionNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				cohesionVector += cohesionNeighbours[i].myTransform.position;
			}
		}

		cohesionVector /= neighboursInFOV;
		cohesionVector -= myTransform.position;
		cohesionVector = cohesionVector.normalized;
		return cohesionVector;
	}

	private Vector3 CalculateAligementVector()
	{
		if (myTransform == null) return new Vector3(0,0,0);

		var aligementVector = myTransform.forward;
		if (aligementNeighbours.Count == 0)
			return myTransform.forward;
		int neighboursInFOV = 0;
		for (int i = 0; i < aligementNeighbours.Count; i++)
		{
			if (IsInFOV(aligementNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				aligementVector += aligementNeighbours[i].myTransform.forward;
			}
		}

		aligementVector /= neighboursInFOV;
		aligementVector = aligementVector.normalized;
		return aligementVector;
	}

	private Vector3 CalculateAvoidanceVector()
	{
		var avoidanceVector = Vector3.zero;
		if (aligementNeighbours.Count == 0)
			return Vector3.zero;
		int neighboursInFOV = 0;
		for (int i = 0; i < avoidanceNeighbours.Count; i++)
		{
			if (IsInFOV(avoidanceNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
			}
		}

		avoidanceVector /= neighboursInFOV;
		avoidanceVector = avoidanceVector.normalized;
		return avoidanceVector;
	}

	private Vector3 CalculateBoundsVector()
	{
		if (myTransform == null) return Vector3.zero;
		var offsetToCenter = assignedFlock.transform.position - myTransform.position;
		bool isNearCenter = (offsetToCenter.magnitude >= assignedFlock.boundsDistance * 0.9f);	
		return isNearCenter ? offsetToCenter.normalized : Vector3.zero;
	}

	private Vector3 CalculateObstacleVector()
	{
		if (myTransform == null) return Vector3.zero;
		var obstacleVector = Vector3.zero;

		//Underscore is gewoon een discard van de variabele aangezien ik die niet nodig heb
        if (Physics.Raycast(myTransform.position, myTransform.forward, out _, assignedFlock.obstacleDistance, obstacleMask))
        {
            obstacleVector = FindBestDirectionToAvoidObstacle();
        }
        else
        {
            currentObstacleAvoidanceVector = Vector3.zero;
        }
        return obstacleVector;
	}

	private Vector3 FindBestDirectionToAvoidObstacle()
	{
		if (currentObstacleAvoidanceVector != Vector3.zero)
		{
            //Underscore is gewoon een discard van de variabele aangezien ik die niet nodig heb
            if (!Physics.Raycast(myTransform.position, myTransform.forward, out _, assignedFlock.obstacleDistance, obstacleMask))
            {
                return currentObstacleAvoidanceVector;
            }
        }
		float maxDistance = int.MinValue;
		var selectedDirection = Vector3.zero;
		for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Length; i++)
		{

            var currentDirection = myTransform.TransformDirection(directionsToCheckWhenAvoidingObstacles[i].normalized);
            if (Physics.Raycast(myTransform.position, currentDirection, out RaycastHit hit, assignedFlock.obstacleDistance, obstacleMask))
			{

				float currentDistance = (hit.point - myTransform.position).sqrMagnitude;
				if (currentDistance > maxDistance)
				{
					maxDistance = currentDistance;
					selectedDirection = currentDirection;
				}
			}
			else
			{
				selectedDirection = currentDirection;
				currentObstacleAvoidanceVector = currentDirection.normalized;
				return selectedDirection.normalized;
			}
		}
		return selectedDirection.normalized;
	}

	private bool IsInFOV(Vector3 position)
	{
		return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
	}
	private void OnDrawGizmos()
	{
        for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Length; i++)
        {
			Vector3 dir = new Vector3(directionsToCheckWhenAvoidingObstacles[i].x, directionsToCheckWhenAvoidingObstacles[i].y, directionsToCheckWhenAvoidingObstacles[i].z);
			Debug.DrawRay(transform.position, dir / 100, Color.green);
		}
		Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
		Gizmos.color = new Color32(41, 0, 255, 35);
		//Gizmos.DrawWireSphere(transform.position, assignedFlock.cohesionDistance);
		Gizmos.color = new Color32(70, 70, 245, 35);
		//Gizmos.DrawWireSphere(transform.position, assignedFlock.aligementDistance);
		Gizmos.color = new Color32(24, 114, 222, 35);
		//Gizmos.DrawWireSphere(transform.position, assignedFlock.avoidanceDistance);
		Gizmos.color = new Color32(39, 178, 245, 35);
		//Gizmos.DrawWireSphere(transform.position, assignedFlock.aligementDistance);
		Gizmos.color = new Color32(28, 250, 192, 35);
		//Gizmos.DrawWireSphere(transform.position, assignedFlock.obstacleDistance);
	}
    private void Start()
    {
        
    }
}