using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode
{
    None,
    Connections,
    Paths
}

public class AIWaypointNetwork : MonoBehaviour
{
    [HideInInspector]
    public PathDisplayMode displayMode = PathDisplayMode.Connections;
    [HideInInspector]
    public int UIStart = 0, UIEnd = 0;

    public List<Transform> Waypoints = new List<Transform> ();
}
