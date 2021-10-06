using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Controllers;
using Mek.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.Behaviours
{
    public enum TeamSide
    {
        Friendly,
        Enemy
    }

    public abstract class Team : MonoBehaviour
    {
        public abstract TeamSide TeamSide { get; }
        
        [ShowInInspector, ReadOnly] protected List<StickMan> _stickMen = new List<StickMan>();
        
        
        private StickMan _stickManPrefab;
        
        public float Length { get; private set; }
        public Team AttackingTo { get; private set; }
        public bool IsAttacking => AttackingTo != null;
        public bool IsLost => _stickMen.Count == 0;

        public List<StickMan> StickMen => _stickMen;

        protected virtual void Awake()
        {
            StickMan.Lost += OnStickManLost;
        }

        protected virtual void Start()
        {
            _stickManPrefab = AssetController.Instance.StickManPrefab;
        }

        public virtual void Initialize(int count)
        {
            PopulateCrowd(count);
        }

        public virtual void Dispose()
        {
            StickMan.Lost -= OnStickManLost;
            
            foreach (var stickMan in _stickMen)
            {
                stickMan.Recycle();
            }
            _stickMen.Clear();
        }
        
        [Button]
        protected void PopulateCrowd(int targetCount)
        {
            var spawnCount = Mathf.Max(0, targetCount - _stickMen.Count);

            for (int i = 0; i < spawnCount; i++)
            {
                // var stickMan = Instantiate(_stickManPrefab, transform);
                var stickMan = _stickManPrefab.Spawn();
                stickMan.transform.SetParent(transform, false);
                
                _stickMen.Add(stickMan);
            }

            for (int i = _stickMen.Count - 1; i >= targetCount; i--)
            {
                var stickMan = _stickMen[i];
                stickMan.Recycle();
                _stickMen.Remove(stickMan);
            }
            
            SetPositions();

            foreach (var stickMan in _stickMen)
            {
                stickMan.Initialize(this);
            }
        }
        
        protected void SetPositions(bool instant = true)
        {
            var targetPositionList =
                GetPositionListAround(Vector3.zero, new float[] {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f}, new int[] {5, 10, 20, 30, 40, 50, 60, 70, 80, 90});

            var targetPositionListIndex = 0;

            Length = 0;
            foreach (var stickMan in _stickMen)
            {
                var targetPos = targetPositionList[targetPositionListIndex];
                if (instant)
                {
                    stickMan.transform.localPosition = targetPos;
                }
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                stickMan.SetTargetPosition(targetPos);

                if (targetPos.x > Length)
                {
                    Length = targetPos.x;
                }
            }
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

        public virtual void DeclareWarWith(Team team)
        {
            if (team.TeamSide == TeamSide || IsAttacking) return;
            AttackingTo = team;
            
            foreach (var stickMan in _stickMen)
            {
                stickMan.Chase();
            }
        }

        protected virtual void OnBrawlSucceed()
        {
            AttackingTo = null;
            ToggleAgents(false);
            // SetPositions(false);
        }

        protected virtual void OnBrawlDefeated()
        {
            AttackingTo = null;
            ToggleAgents(false);
            // SetPositions(false);
        }

        private void ToggleAgents(bool state)
        {
            foreach (var stickMan in _stickMen)
            {
                stickMan.ToggleAgent(state);
            }
        }

        protected virtual void OnStickManLost(StickMan stickMan)
        {
            // stickMan.Recycle();
            _stickMen.Remove(stickMan);
        }

        protected virtual void FixedUpdate()
        {
            if (!IsAttacking) return;

            if (AttackingTo.IsLost)
            {
                OnBrawlSucceed();
            }
            else if (IsLost)
            {
                OnBrawlDefeated();
            }
        }

        public StickMan GetAStickManToChase()
        {
            if (!AttackingTo) return null;
            
            var stickMan = AttackingTo.StickMen.Find(s => !s.HasChased);

            if (!stickMan)
            {
                stickMan = AttackingTo.StickMen.RandomElement();
            }

            return stickMan;
        }

        private void OnTriggerEnter(Collider other)
        {
        }
    }
}
