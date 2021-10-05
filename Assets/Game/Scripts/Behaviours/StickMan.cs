using System;
using System.Collections;
using DG.Tweening;
using Game.Scripts.Models;
using Mek.Extensions;
using Mek.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Behaviours
{
    public class StickMan : MonoBehaviour, IRecycleCallbackReceiver
    {
        public static event Action FinishLinePassed;
        public static event Action<StickMan> Hit;
        public static event Action<StickMan> Smashed;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Animator _animator;
        
        private Vector3? _direction;
        private Vector3? _targetPosition;

        private Color _colorFriendly;
        private Color _colorEnemy;

        private const float HorizontalSpeed = 500f;

        private bool _isChasing;

        private int _runningAnimationID => Animator.StringToHash("IsRunning");

        private Team _team;
        
        private bool Active { get; set; }
        private bool IsFinishing { get; set; }

        private void Awake()
        {
            // _agent.SetDestination(transform.position + Vector3.forward * 0.1f);
            _colorFriendly = GameConfig.StickManColorFriendly;
            _colorEnemy = GameConfig.StickManColorEnemy;
            // _rigidbody.isKinematic = true;
        }

        public void Initialize(Team team)
        {
            _team = team;
            _renderer.material.color = _team.TeamSide == TeamSide.Friendly
                ? _colorFriendly
                : _colorEnemy;

            // _rigidbody.isKinematic = false;

            gameObject.layer = LayerMask.NameToLayer(_team.TeamSide == TeamSide.Friendly ? "Player" : "Enemy");
            Active = true;
            IsFinishing = false;
        }

        public void SetTargetPosition(Vector3 pos)
        {
            _targetPosition = pos;
        }

        private void FixedUpdate()
        {
            if (_agent.enabled) return;

            // if (_isChasing && _targetPosition.HasValue)
            // {
            //     transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition.Value, Time.fixedDeltaTime);
            // }
            var relativePos = Vector3.zero;
            if (_targetPosition.HasValue)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition.Value, Time.fixedDeltaTime);
                // relativePos = _targetPosition.Value - transform.position;
            }
            
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, Vector3.zero, Time.fixedDeltaTime * 10f);

            // transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            
            
            
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

        private void OnTriggerEnter(Collider other)
        {
            if (!Active) return;
            
            if (other.gameObject.CompareTag("Obstacle"))
            {
                Hit?.Invoke(this);
            }
            else if (other.gameObject.CompareTag("Team"))
            {
                var team = other.gameObject.GetComponent<Team>();

                if (!team) return;
                if (team.TeamSide == _team.TeamSide) return;
                _team.DeclareWarWith(team);
                team.DeclareWarWith(_team);
            }
            else if (other.gameObject.CompareTag("Finish"))
            {
                Debug.Log("finishLinePassed");
                OnFinishLinePassed();
                FinishLinePassed?.Invoke();
            }
        }

        private void OnFinishLinePassed()
        {
            IsFinishing = true;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("StickMan"))
            {
                var otherStickMan = other.gameObject.GetComponent<StickMan>();
                if (otherStickMan._team.TeamSide == _team.TeamSide) return;
                Debug.Log("Dead");
                OnSmashed();
                this.Recycle();
            }
        }

        public void MoveToDestinationWithAgent(Vector3 position)
        {
            ToggleAgent(true);
            // _targetPosition = position;
            _isChasing = true;

            _agent.SetDestination(position);
        }

        public void ToggleAgent(bool state)
        {
            // _isChasing = state;
            // return;
            // _rigidbody.isKinematic = state;
            if (state && _agent.enabled)
            {
                _agent.ResetPath();
            }
            _agent.enabled = state;
        }

        private void OnSmashed()
        {
            Smashed?.Invoke(this);
        }

        public void OnRecycle()
        {
            Active = false;
            // _rigidbody.isKinematic = true;
        }
    }
}
