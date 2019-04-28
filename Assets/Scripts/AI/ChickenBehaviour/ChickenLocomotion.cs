using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

[RequireComponent(typeof(StateManager))]
public class ChickenLocomotion : Unit
{
    private StateManager _stateManager;
    public float RotationSpeed = 3f;

    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
        _stateManager.OnDestinationFound += DestinationFound;
    }

    protected override void Start()
    {
    }

    private void DestinationFound(Vector3 destination)
    {
        PathRequestManager.RequestPath(new PathRequest(transform.position, destination, OnPathFound));
    }

    private void OnDisable()
    {
        _stateManager.OnDestinationFound -= DestinationFound;
    }

    protected override IEnumerator FollowPath()
    {
        if(_path.Length == 0)
        {
            _stateManager.IsAtDestination = true;
            yield break;
        }

        Vector3 currentWaypoint = _path[0];
        _targetIndex = 0;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    _stateManager.IsAtDestination = true;
                    yield break;
                }
                
                currentWaypoint = _path[_targetIndex];
            }

            float delta = TimeScaleManager.Instance != null ? TimeScaleManager.Delta : Time.deltaTime;
            float step = RotationSpeed * delta;
            Vector3 targetDir = currentWaypoint - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * delta);
            yield return null;
        }
    }
}
