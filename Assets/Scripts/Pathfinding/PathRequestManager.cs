using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathRequestManager : MonoBehaviour
{
    struct PathRequest
    {
        public Vector3 start;
        public Vector3 end;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 s, Vector3 e, Action<Vector3[], bool> call)
        {
            start = s;
            end = e;
            callback = call;
        }
    }

    private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    private PathRequest _currentPathRequest;
    private AStarPathfinding _pathfinding;

    private bool _isProcessingPath;

    private static PathRequestManager _instance;

    public static PathRequestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No instance of X360_InputManager");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

        _pathfinding = GetComponent<AStarPathfinding>();
    }

    public static void RequestPath(Vector3 start, Vector3 target, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(start, target, callback);
        _instance._pathRequestQueue.Enqueue(newRequest);
        _instance.TryProcessNextRequest();
    }

    public void TryProcessNextRequest()
    {
        if (!_isProcessingPath && _pathRequestQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessingPath = true;
            _pathfinding.StartFindPath(_currentPathRequest.start, _currentPathRequest.end);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        _currentPathRequest.callback(path, success);
        _isProcessingPath = false;
        TryProcessNextRequest();
    }
}
