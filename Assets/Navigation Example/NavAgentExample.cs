using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    public AIWaypointNetwork waypointNetwork = null;
    public int currentIndex = 0;
    public bool hasPath = false;
    public bool pathPending = false;
    public bool isPathStale = false;
    public NavMeshPathStatus pathStatus = NavMeshPathStatus.PathInvalid;

    NavMeshAgent _navAgent = null;

	void Start () {
        _navAgent = GetComponent<NavMeshAgent> ();
        if (waypointNetwork == null) return;
        SetNextDestination (false);
	}
	
    void SetNextDestination (bool increment)
    {
        if (!waypointNetwork) return;

        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;

        int nextWaypoint = (currentIndex + incStep >= waypointNetwork.Waypoints.Count) ? 0 : currentIndex + incStep;
        nextWaypointTransform = waypointNetwork.Waypoints[nextWaypoint];
        if (nextWaypointTransform != null)
        {
            currentIndex = nextWaypoint;
            _navAgent.destination = nextWaypointTransform.position;
            return;
        }
        currentIndex++;
    }

	void Update () {
        hasPath = _navAgent.hasPath;
        pathPending = _navAgent.pathPending;
        isPathStale = _navAgent.isPathStale;
        pathStatus = _navAgent.pathStatus;

        if ((!hasPath && !pathPending) || pathStatus == NavMeshPathStatus.PathInvalid /*|| pathStatus == NavMeshPathStatus.PathPartial*/)
        {
            SetNextDestination (true);
        }
        else if (isPathStale)
        {
            SetNextDestination (false);
        }
    }
}
