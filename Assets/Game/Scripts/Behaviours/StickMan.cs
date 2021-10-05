using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Behaviours
{
    public class StickMan : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        
        private Vector3? _direction;
        private Vector3? _targetPosition;

        private const float HorizontalSpeed = 500f;

        private int _runningAnimationID => Animator.StringToHash("IsRunning");

        private void Awake()
        {
            // _agent.SetDestination(transform.position + Vector3.forward * 0.1f);
        }

        public void LeanTowards(Vector3 direction)
        {
            _direction = direction;
        }

        public void CancelLeaning()
        {
            _direction = null;
            _targetPosition = null;
        }

        public void SetTargetPosition(Vector3 pos)
        {
            _targetPosition = pos;
        }

        private void FixedUpdate()
        {
            if (_targetPosition.HasValue)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition.Value, Time.fixedDeltaTime);
            } 
            // var horizontalDelta = Vector3.zero;
            //
            // if (_direction.HasValue && _direction.Value != Vector3.zero)
            // {
            //     var currentPosition = transform.position;
            //     // var newPosition = Vector3.MoveTowards(currentPosition, currentPosition + _direction.Value, Mathf.Infinity);
            //     var newPosition = Vector3.MoveTowards(currentPosition, currentPosition + _direction.Value, Mathf.Infinity);
            //
            //     horizontalDelta = Vector3.Lerp(Vector3.zero, newPosition - currentPosition, HorizontalSpeed * Time.fixedDeltaTime);
            //
            //     // var lerpedPosition = Mathf.Lerp(currentPosition.x, newPosition.x, HorizontalSpeed * Time.fixedDeltaTime);
            // }
            //
            //
            // _agent.SetDestination(transform.position + Vector3.forward * 1f  + horizontalDelta * 5);
        }
        
        public void SetRunning(bool state)
        {
            _animator.SetBool(_runningAnimationID, state);
        }
    }
}
