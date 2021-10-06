using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Controllers;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class FriendlyTeam : Team
    {
        public event Action PassedFinishLine;
        public event Action Failed;
        
        private Vector3? _direction;

        private const float ForwardSpeed = 10f;
        private const float HorizontalSpeed = 1f;

        public override TeamSide TeamSide => TeamSide.Friendly;

        public bool IsRunning { get; private set; }
        public bool IsFinished { get; private set; }
        public bool HasPassedFinishLine { get; private set; }

        private Transform _followPoint;

        private List<List<StickMan>> _triangularStickMan = new List<List<StickMan>>();

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Initialize(int count)
        {
            base.Initialize(count);
            IsFinished = false;
            HasPassedFinishLine = false;
            SetRunning(true);
            
            StickMan.FinishLinePassed += OnFinishLinePassed;
            StickMan.Lost += OnStickManLost;
            ExaminationBlocks.ExaminationCompleted += OnExaminationCompleted;

            if (_followPoint == null)
            {
                _followPoint = new GameObject("FollowPoint").transform;
                _followPoint.SetParent(transform, false);
            }
            
            CameraController.Instance.Follow(_followPoint);
            CameraController.Instance.ChangeState(CameraState.Running);
        }

        public override void Dispose()
        {
            base.Dispose();
            IsFinished = false;
            HasPassedFinishLine = false;

            StickMan.FinishLinePassed -= OnFinishLinePassed;
            StickMan.Lost -= OnStickManLost;
            ExaminationBlocks.ExaminationCompleted -= OnExaminationCompleted;
            
            _followPoint.transform.localPosition = Vector3.zero;
            _triangularStickMan.Clear();
        }

        private void OnFinishLinePassed()
        {
            if (HasPassedFinishLine) return;
            HasPassedFinishLine = true;
            PassedFinishLine?.Invoke();
            _countContaier.gameObject.SetActive(false);
            
            // CreateTriangle();
        }

        private void OnStickMenStopped()
        {
            IsFinished = true;
        }

        protected override void FixedUpdate()
        {
            if (IsFinished) return;
            base.FixedUpdate();
            if (IsAttacking) return;
            var horizontalDelta = Vector3.zero;
            var currentPosition = transform.position;

            if (_stickMen.All(s => s.IsStopped))
            {
                OnStickMenStopped();
            }
            
            if (_direction.HasValue && _direction.Value != Vector3.zero)
            {
                horizontalDelta = _direction.Value;
            }

            var targetPos = Vector3.zero;
            if (HasPassedFinishLine)
            {
                targetPos = Vector3.Lerp(currentPosition, new Vector3(0, currentPosition.y, currentPosition.z) + Vector3.forward * ForwardSpeed, Time.fixedDeltaTime);
            }
            else
            {
                targetPos = Vector3.Lerp(currentPosition, currentPosition + Vector3.forward * ForwardSpeed + horizontalDelta * HorizontalSpeed, Time.fixedDeltaTime);
            }
            
            targetPos.x = Mathf.Clamp(targetPos.x, -GameConfig.Bounds + Length, GameConfig.Bounds - Length);
            transform.position = targetPos;
        }
        
        public void LeanTowards(Vector3 direction)
        {
            _direction = direction;
        }

        public void CancelLeaning()
        {
            _direction = null;
        }

        private void OnExaminationCompleted(ExaminationData data)
        {
            Debug.Log($"Triggered: {data.Operation}{data.Value}");
            
            PopulateCrowd(GetCrowdCountAfterEvaluateOperation(data));
            
            SetRunning(true);
        }

        private int GetCrowdCountAfterEvaluateOperation(ExaminationData data)
        {
            int targetCount = _stickMen.Count;
            switch (data.Operation)
            {
                case MathOperation.Addition:
                    targetCount += Mathf.FloorToInt(data.Value);
                    break;
                case MathOperation.Subtraction:
                    targetCount -= Mathf.FloorToInt(data.Value);
                    break;
                case MathOperation.Multiplication:
                    targetCount *= Mathf.FloorToInt(data.Value);
                    break;
                case MathOperation.Division:
                    targetCount /= Mathf.FloorToInt(data.Value);
                    break;
            }

            return Mathf.Max(0, targetCount);
        }

        protected override void OnStickManLost(StickMan stickMan)
        {
            base.OnStickManLost(stickMan);
            if (_stickMen.Count != 0 || IsFinished) return;
            IsFinished = true;
            Failed?.Invoke();
        }

        private void SetRunning(bool state)
        {
            foreach (var stickMan in _stickMen)
            {
                stickMan.SetRunning(state);
            }

            IsRunning = state;
        }

        public override void DeclareWarWith(Team team)
        {
            base.DeclareWarWith(team);

            IsRunning = false;
        }

        protected override void OnBrawlSucceed()
        {
            base.OnBrawlSucceed();
            
            SetPositions(false);
            IsRunning = true;
        }

        protected override void OnBrawlDefeated()
        {
            base.OnBrawlDefeated();
            
            SetPositions(false);
            IsRunning = true;
        }
        public List<int> GetStairsList()
        {
            var stickManTowerRowCountList = new List<int>();
            var remainingCount = _stickMen.Count;
            var index = 0;

            while (remainingCount > 0)
            {
                var stickManCount = Mathf.RoundToInt(0.220576f * Mathf.Pow(index, 1.24525f) + 0.796018f);
                Debug.Log(stickManCount);
                index++;
                remainingCount -= stickManCount;
                stickManTowerRowCountList.Add(stickManCount);
            }
            
            return stickManTowerRowCountList;
        }
        public void CreateTriangle()
        {
            var stairsList = GetStairsList();
            var remainingCount = _stickMen.Count;

            stairsList.Reverse();

            var index = 0;
            var lastY = 0f;
            for (int i = 0; i < stairsList.Count && remainingCount > 0; i++)
            {
                var stickManCount = stairsList[i];
                var bounds = stickManCount / 2f - 0.5f;

                var stickMen = new List<StickMan>();
                
                for (int j = 0; j < stickManCount && remainingCount > 0; j++)
                {
                    var pos = new Vector3(-bounds + (1 * j), 1.8f * i, 0);
            
                    // var stickMan = Instantiate(_stickManPrefabbb, pos, Quaternion.identity, transform);
                    // _stickMen.Add(stickMan);

                    var stickMan = _stickMen[index];
                    stickMen.Add(stickMan);
                    
                    // stickMan.SetTargetPosition(pos); // todo: call tower method
                    stickMan.FormUpAsTower(pos, index * 0.01f);
                    index++;
                    remainingCount--;
                    lastY = pos.y;
                }
                
                _triangularStickMan.Add(stickMen);
            }
            
            CameraController.Instance.FollowTriangularStickMen(_triangularStickMan, index * 0.05f);

            _followPoint.DOLocalMove(new Vector3(0, lastY, 0), 1.5f)
                .SetLoops(2, LoopType.Yoyo);
            
            CameraController.Instance.Follow(_followPoint);
        }
    }
}
