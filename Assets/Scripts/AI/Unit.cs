using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    private Vector3[] _path;
    private int _targetIndex;

    private void OnDrawGizmos()
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

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, Target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool success)
    {
        if (success)
        {
            _path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
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

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);
            yield return null;
        }
    }
}
