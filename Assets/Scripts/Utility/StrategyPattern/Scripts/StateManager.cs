using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        public delegate void PathRequestToDestination(Vector3 destination);
        public event PathRequestToDestination OnDestinationFound;

        public State currentState;
        [Header("Animation")]
        public Animator Animator;

        public AnimHashes Hashes;

        public float IdleTimer;
        public float WaitTime;
        public float delta;
        public bool IsAtDestination = true;

        private void Awake()
        {
            if(Animator == null)
                Animator = GetComponentInChildren<Animator>();
            WaitTime = Random.Range(ChickenConstants.WaitIdleRange.rangeStart, ChickenConstants.WaitIdleRange.rangeEnd);
        }

        private void Start()
        {
            Hashes = new AnimHashes();

            if(currentState != null)
            {
                currentState.OnEnter(this);
            }
        }

        private void FixedUpdate()
        {
            delta = TimeScaleManager.FixedDelta;
            if(currentState != null)
            {
                currentState.FixedTick(this);
            }
        }
        
        private void Update()
        {
            delta = TimeScaleManager.Delta;
            if(currentState != null)
            {
                currentState.Tick(this);
            }
            Animator.speed = TimeScaleManager.TimeScale;
        }

        public void GoToDestination(Vector3 destination)
        {
            if(OnDestinationFound != null)
            {
                IsAtDestination = false;
                OnDestinationFound(destination);
            }
            else
            {
                Debug.LogError("OnDestinationFound is empty");
            }
        }
    }
}
