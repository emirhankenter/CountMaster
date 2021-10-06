using System;
using System.Collections;
using DG.Tweening;
using Game.Scripts.Models;
using Mek.Coroutines;
using Mek.Extensions;
using Mek.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Behaviours
{
    public class StickMan : MonoBehaviour, IRecycleCallbackReceiver
    {
        public static event Action FinishLinePassed;
        public static event Action<StickMan> Lost;
        
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
        public bool HasChased { get; private set; }
        public bool IsFormingAsTower { get; private set; }
        public bool IsStopped { get; private set; }

        private string _chaseRoutineKey => $"{GetInstanceID()}chaseRoutine";
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
            if (IsFormingAsTower) return;
            var relativePos = Vector3.zero;
            if (_targetPosition.HasValue)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition.Value, Time.fixedDeltaTime);
                // relativePos = _targetPosition.Value - transform.position;
            }
            
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, Vector3.zero, Time.fixedDeltaTime * 10f);
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
                OnHit();
            }
            else if (other.gameObject.CompareTag("Team"))
            {
                var team = other.gameObject.GetComponent<Team>();

                if (!team) return;
                if (team.TeamSide == _team.TeamSide) return;
                _team.DeclareWarWith(team);
                team.DeclareWarWith(_team);
            }
            else if (other.gameObject.CompareTag("FinishLine"))
            {
                if (_team.TeamSide != TeamSide.Friendly || !(_team is FriendlyTeam friendlyTeam)) return;
                if (friendlyTeam.HasPassedFinishLine) return;
                Debug.Log("finishLinePassed");
                OnFinishLinePassed();
                FinishLinePassed?.Invoke();
            }
            else if (other.gameObject.CompareTag("StairTrigger"))
            {
                transform.SetParent(null, true);
                SetRunning(false);
                IsStopped = true;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("StickMan"))
            {
                var otherStickMan = other.gameObject.GetComponent<StickMan>();
                if (otherStickMan._team.TeamSide == _team.TeamSide) return;
                OnSmashed();
            }
        }

        private void OnSmashed()
        {
            this.Recycle();
            Lost?.Invoke(this);
            if (CoroutineController.IsCoroutineRunning(_chaseRoutineKey))
            {
                CoroutineController.StopCoroutine(_chaseRoutineKey);
            }
        }

        private void OnHit()
        {
            this.Recycle();
            Lost?.Invoke(this);
            if (CoroutineController.IsCoroutineRunning(_chaseRoutineKey))
            {
                CoroutineController.StopCoroutine(_chaseRoutineKey);
            }
        }

        private void OnFinishLinePassed()
        {
            IsFinishing = true;
        }

        public void Chase()
        {
            var target = _team.GetAStickManToChase();
            if (!target) return;
            ToggleAgent(true);
            // _targetPosition = position;
            _isChasing = true;

            if (CoroutineController.IsCoroutineRunning(_chaseRoutineKey))
            {
                CoroutineController.StopCoroutine(_chaseRoutineKey);
            }
            ChaseRoutine().StartCoroutine(_chaseRoutineKey);
            // _agent.SetDestination(position);
        }
            
        IEnumerator ChaseRoutine()
        {
            var target = _team.GetAStickManToChase();
            while (true)
            {
                if (!Active) yield break;
                if (!target.Active)
                {
                    target = _team.GetAStickManToChase();
                }

                if (target == null) yield break;
                
                _agent.SetDestination(target.transform.position);

                yield return new WaitForSeconds(1f);
            }
        }

        public void ToggleAgent(bool state)
        {
            // _isChasing = state;
            // return;
            // _rigidbody.isKinematic = state;

            if (!state)
            {
                if (CoroutineController.IsCoroutineRunning(_chaseRoutineKey))
                {
                    CoroutineController.StopCoroutine(_chaseRoutineKey);
                }
            }
            
            if (state && _agent.enabled)
            {
                _agent.ResetPath();
            }
            _agent.enabled = state;
        }

        public void FormUpAsTower(Vector3 targetLocalPosition, float delay)
        {
            if (IsFormingAsTower) return;
            IsFormingAsTower = true;

            // transform.localPosition = targetLocalPosition;
            
            // if (CoroutineController.IsCoroutineRunning(_formUpRoutineKey))
            // {
            //     CoroutineController.StopCoroutine(_formUpRoutineKey);
            // }
            // FormUpAsTowerRoutine().StartCoroutine(_formUpRoutineKey);

            transform.DOLocalMove(targetLocalPosition, 0.01f)
                .SetDelay(delay)
                .SetEase(Ease.Linear);

            _targetPosition = targetLocalPosition;
        }

        public void OnRecycle()
        {
            Active = false;
            _isChasing = false;
            HasChased = false;
            IsFinishing = false;
            IsFormingAsTower = false;
            IsStopped = false;
            _agent.enabled = false;
        }
    }
}
