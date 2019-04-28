using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform Target;
    public float Speed;

    protected Vector3[] _path;
    protected int _targetIndex;

    protected virtual void OnDrawGizmos()
    {
        if(_path != null)
        {
            for(int i = _targetIndex; i < _path.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(_path[i], Vector3.one);

                if(i == _targetIndex)
                {
                    Gizmos.DrawLine(transform.position, _path[i]);
                }
                else
                {
                    Gizmos.DrawLine(_path[i - 1], _path[i]);
                }
            }
        }
    }

    protected virtual void Start()
    {
        PathRequestManager.RequestPath(new PathRequest(transform.position, Target.position, OnPathFound));
    }

    protected void OnPathFound(Vector3[] newPath, bool success)
    {
        if (success)
        {
            _path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    protected virtual IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];
        _targetIndex = 0;
        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                _targetIndex++;
                if(_targetIndex >= _path.Length)
                {
                    yield break;
                }
                currentWaypoint = _path[_targetIndex];
            }

            float delta = TimeScaleManager.Instance != null ? TimeScaleManager.Delta : Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * delta);
            yield return null;
        }
    }
}
