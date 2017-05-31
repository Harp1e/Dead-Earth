using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState
{
    Open,
    Animating,
    Closed
}
public class SlidingDoorDemo : MonoBehaviour 
{
    public float slidingDistance = 4.0f;
    public float duration = 1.5f;
    public AnimationCurve slideCurve = new AnimationCurve ();

    Transform _transform = null;
    Vector3 _openPos = Vector3.zero;
    Vector3 _closedPos = Vector3.zero;
    DoorState _doorState = DoorState.Closed;

	void Start () {
        _transform = transform;
        _closedPos = _transform.position;
        _openPos = _closedPos + (_transform.right * slidingDistance);
	}
	
	void Update () {
        if (Input.GetKeyDown (KeyCode.Space) && _doorState != DoorState.Animating)
        {
            StartCoroutine (AnimateDoor ((_doorState == DoorState.Open) ? DoorState.Closed : DoorState.Open));
        }
	}

    IEnumerator AnimateDoor (DoorState newState)
    {
        _doorState = DoorState.Animating;
        float time = 0.0f;
        Vector3 startPos = (newState == DoorState.Open) ? _closedPos : _openPos;
        Vector3 endPos = (newState == DoorState.Open) ? _openPos : _closedPos;
        while (time <= duration)
        {
            float t = time / duration;
            _transform.position = Vector3.Lerp (startPos, endPos, slideCurve.Evaluate(t));
            time += Time.deltaTime;
            yield return null;
        }
        _transform.position = endPos;
        _doorState = newState;
    }
}
