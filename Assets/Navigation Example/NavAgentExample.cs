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
    public AnimationCurve jumpCurve = new AnimationCurve ();

    bool handlingOffMeshLink = false;
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

       if (_navAgent.isOnOffMeshLink && !handlingOffMeshLink)
        {
            handlingOffMeshLink = true;
            StartCoroutine (Jump (1.0f));
            return;
        }

//  replace !hasPath with remainingDistance check to overcome offmeshlink bug that invalidates path 1 frame after link
        if ((_navAgent.remainingDistance <= _navAgent.stoppingDistance && !pathPending) || pathStatus == NavMeshPathStatus.PathInvalid /*|| pathStatus == NavMeshPathStatus.PathPartial*/)
        {
            SetNextDestination (true);
        }
        else if (isPathStale)
        {
            SetNextDestination (false);
        }
    }

    IEnumerator Jump (float duration)
    {
        OffMeshLinkData data = _navAgent.currentOffMeshLinkData;
        Vector3 startPos = _navAgent.transform.position;
        Vector3 endPos = data.endPos + (_navAgent.baseOffset * Vector3.up);
        float time = 0.0f;

        while (time <= duration)
        {
            float t = time / duration;
            _navAgent.transform.position = Vector3.Lerp (startPos, endPos, t) + (jumpCurve.Evaluate(t) * Vector3.up);
            time += Time.deltaTime;
            yield return null;
        }
        handlingOffMeshLink = false;
        _navAgent.CompleteOffMeshLink ();
    }
}
