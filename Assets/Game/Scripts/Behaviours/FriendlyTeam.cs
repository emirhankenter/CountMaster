using System;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using Mek.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class FriendlyTeam : Team
    {
        public event Action<bool> Finished;
        
        private Vector3? _direction;

        private const float ForwardSpeed = 10f;
        private const float HorizontalSpeed = 1500f;

        public override TeamSide TeamSide => TeamSide.Friendly;

        public bool IsRunning { get; private set; }
        public bool IsFinished { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            StickMan.FinishLinePassed += OnFinishLinePassed;
            StickMan.Hit += OnHitToObstacle;
            ExaminationBlocks.ExaminationCompleted += OnExaminationCompleted;
        }

        protected override void Start()
        {
            base.Start();
            
            Initialize(1);

            CameraController.Instance.Follow(transform);
        }

        public override void Initialize(int count)
        {
            base.Initialize(count);
            IsFinished = false;
            SetRunning(true);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            StickMan.FinishLinePassed -= OnFinishLinePassed;
            StickMan.Hit -= OnHitToObstacle;
            ExaminationBlocks.ExaminationCompleted -= OnExaminationCompleted;
        }

        private void OnFinishLinePassed()
        {
            IsFinished = true;
            Finished?.Invoke(true);
            
            CreateTriangle();
        }

        protected override void FixedUpdate()
        {
            if (IsFinished) return;
            base.FixedUpdate();
            if (IsAttacking) return;
            var horizontalDelta = Vector3.zero;
            var currentPosition = transform.position;
            
            if (_direction.HasValue && _direction.Value != Vector3.zero)
            {
                horizontalDelta = _direction.Value;
            }

            var targetPos = Vector3.Lerp(currentPosition, currentPosition + Vector3.forward * ForwardSpeed + horizontalDelta * HorizontalSpeed, Time.fixedDeltaTime);
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

        private void OnHitToObstacle(StickMan stickMan)
        {
            stickMan.Recycle();
            _stickMen.Remove(stickMan);

            if (_stickMen.Count == 0)
            {
                IsFinished = true;
                Finished?.Invoke(false);
            }
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
            
            IsRunning = true;
        }

        protected override void OnBrawlDefeated()
        {
            base.OnBrawlDefeated();
            
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

#if UNITY_EDITOR
        
        private void CreateTriangle()
        {
            foreach (var stickMan in _stickMen)
            {
                DestroyImmediate(stickMan.gameObject);
            }
            _stickMen.Clear();

            var stickManCountList = GetStairsList();
            var remainingCount = _stickMen.Count;

            stickManCountList.Reverse();

            var index = 0;
            for (int i = 0; i < stickManCountList.Count; i++)
            {
                var stickManCount = stickManCountList[i];
                var bounds = stickManCount / 2f - 0.5f;
                
                for (int j = 0; j < stickManCount && remainingCount > 0; j++)
                {
                    var pos = new Vector3(-bounds + (1 * j), 1.8f * i, 0);
            
                    // var stickMan = Instantiate(_stickManPrefabbb, pos, Quaternion.identity, transform);
                    // _stickMen.Add(stickMan);

                    var stickMan = _stickMen[index];
                    
                    stickMan.transform.localPosition = pos; // todo: call tower method
                    index++;
                    remainingCount--;
                }
            }
            //
            // for (int i = stickManCountList.Count - 1; i >= 0; i--)
            // {
            //     var stickManCount = stickManCountList[i];
            //     
            //     for (int j = 0; j < stickManCount; j++)
            //     {
            //         var pos = new Vector3(1 * j, 2 * i, 0);
            //
            //         var stickMan = Instantiate(_stickManPrefabbb, pos, Quaternion.identity, transform);
            //         _stickMen.Add(stickMan);
            //     }
            // }
        }
#endif
    }
}
