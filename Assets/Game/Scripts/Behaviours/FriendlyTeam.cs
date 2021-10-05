using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class FriendlyTeam : Team
    {
        [ShowInInspector, ReadOnly] private List<StickMan> _stickMen = new List<StickMan>();

        private Vector3? _direction;
        
        private StickMan _stickManPrefab;

        private const float ForwardSpeed = 10f;
        private const float HorizontalSpeed = 1500f;

        private Transform _followPoint;
        
        public float Length { get; private set; }
        
        private void Awake()
        {
        }

        private void Start()
        {
            _stickManPrefab = AssetController.Instance.StickManPrefab;
            Initialize(10);

            _followPoint = _stickMen.First().transform;
            CameraController.Instance.Follow(_followPoint);
        }

        public void Initialize(int count)
        {
            PopulateCrowd(count);
            // for (int i = 0; i < count; i++)
            // {
            //     var stickMan = Instantiate(_stickManPrefab, transform);
            //     
            //     _stickMen.Add(_stickManPrefab);
            // }

            foreach (var stickMan in _stickMen)
            {
                stickMan.SetRunning(true);
            }
        }

        [Button]
        private void PopulateCrowd(int targetCount)
        {
            var spawnCount = targetCount - _stickMen.Count;

            for (int i = 0; i < spawnCount; i++)
            {
                var stickMan = Instantiate(_stickManPrefab, transform);
                
                _stickMen.Add(stickMan);
            }
            
            SetPositions();
        }

        private void SetPositions()
        {
            var targetPositionList =
                GetPositionListAround(Vector3.zero, new float[] {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f}, new int[] {5, 10, 20, 30, 40, 50, 60, 70, 80, 90});

            var targetPositionListIndex = 0;

            Length = 0;
            foreach (var stickMan in _stickMen)
            {
                var targetPos = targetPositionList[targetPositionListIndex];
                stickMan.transform.localPosition = targetPos;
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                stickMan.SetTargetPosition(targetPos);

                if (targetPos.x > Length)
                {
                    Length = targetPos.x;
                }
            }
            Debug.Log(Length);
        }
        
        public void LeanTowards(Vector3 direction)
        {
            _direction = direction;
            // foreach (var stickMan in _stickMen)
            // {
            //     stickMan.LeanTowards(direction);
            // }
        }

        public void CancelLeaning()
        {
            _direction = null;
            // foreach (var stickMan in _stickMen)
            // {
            //     stickMan.CancelLeaning();
            // }
        }
        
        private void FixedUpdate()
        {
            var horizontalDelta = Vector3.zero;
            var currentPosition = transform.position;
            
            if (_direction.HasValue && _direction.Value != Vector3.zero)
            {
                var newPosition = Vector3.MoveTowards(currentPosition, currentPosition + _direction.Value, Mathf.Infinity);

                horizontalDelta = Vector3.Lerp(Vector3.zero, newPosition - currentPosition, 50 * Time.fixedDeltaTime);
                horizontalDelta = newPosition - currentPosition;
                horizontalDelta = _direction.Value;

                // var lerpedPosition = Mathf.Lerp(currentPosition.x, newPosition.x, HorizontalSpeed * Time.fixedDeltaTime);
            }

            var targetPos = Vector3.Lerp(currentPosition, currentPosition + Vector3.forward * ForwardSpeed + horizontalDelta * HorizontalSpeed, Time.fixedDeltaTime);
            targetPos.x = Mathf.Clamp(targetPos.x, -GameConfig.Bounds + Length, GameConfig.Bounds - Length);
            transform.position = targetPos;
        }


        private void LateUpdate()
        {
            // var followPointPos = _followPoint.position;
            // followPointPos.x = 0;
            // _followPoint.position = followPointPos;
        }

        private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
        {
            List<Vector3> positionList = new List<Vector3>(){startPosition};

            for (int i = 0; i < ringDistanceArray.Length; i++)
            {
                positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
            }
            
            return positionList;
        }

        private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
        {
            List<Vector3> positionList = new List<Vector3>();

            for (int i = 0; i < positionCount; i++)
            {
                float angle = i * (360f / positionCount);
                Vector3 dir = ApplyRotationToVector(new Vector3(1,0), angle);
                Vector3 position = startPosition + dir * distance;
                positionList.Add(position);
            }
            
            return positionList;
        }

        private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
        {
            return Quaternion.Euler(0, angle, 0) * vec;
        }
    }
}
