using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public struct PathRequest
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

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}

[RequireComponent(typeof(AStarPathfinding))]
public class PathRequestManager : MonoBehaviour
{
    private AStarPathfinding _pathfinding;
    private Queue<PathResult> _resultQueue;
    private PathfindingGrid _grid;

    private static PathRequestManager _instance;

    public static PathRequestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No instance of PathRequestManager");
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
        _grid = GetComponent<PathfindingGrid>();
        _resultQueue = new Queue<PathResult>();
    }

    private void Update()
    {
        if(_resultQueue.Count > 0)
        {
            int itemsCount = _resultQueue.Count;
            lock (_resultQueue)
            {
                for(int i = 0; i < itemsCount; i++)
                {
                    PathResult result = _resultQueue.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            _instance._pathfinding.FindPath(request, _instance.FinishedProcessingPath);
        };

        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (_resultQueue)
        {
            _resultQueue.Enqueue(result);
        }
        
    }

    public static Node GetNode(Vector3 position)
    {
        return _instance._grid.WorldPositionToNode(position);
    }
}
