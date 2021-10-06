using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Controllers;
using Mek.Coroutines;
using Mek.Extensions;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class StairsStage : StageBehaviour
    {
        public static event Action<float> Multiplied;

        private PlayerController _player;

        private string _evaluationRoutineKey => $"evaluationRoutine{GetInstanceID()}";

        private float _stairsCount;

        private List<Stair> _stairs = new List<Stair>();
        
        private static Stair _stairPrefab;

        [SerializeField] private LayerMask _layerMask;
        
        private void Awake()
        {
            if (_stairPrefab == null)
            {
                _stairPrefab = AssetController.Instance.StairPrefab;
            }
        }

        public void SetData(PlayerController playerController, int stairsCount)
        {
            _player = playerController;
            _stairsCount = stairsCount;
        }

        public override void Init()
        {
            // _player = GameController.Instance.CurrentLevel.Player;
            base.Init();
            
            Debug.Log("StairsStage");

            CreateStairs();
            StartEvaluation();
        }

        private void CreateStairs()
        {
            var colors = AssetController.Instance.StairColors;
            var temp = _stairsCount + 5;
            for (int i = 0; i < temp; i++)
            {
                var stair = _stairPrefab.Spawn();
                var stairT = stair.transform;
                stairT.SetParent(transform, false);
                stairT.localPosition = new Vector3(0, i * Stair.Height, i * Stair.Lenght);

                var color = colors[i % colors.Count];
                var multiplier = 1 + 0.1f * i;
                stair.Initialize(multiplier, color);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (CoroutineController.IsCoroutineRunning(_evaluationRoutineKey))
            {
                CoroutineController.StopCoroutine(_evaluationRoutineKey);
            }

            foreach (var stair in _stairs)
            {
                stair.Recycle();
            }
            
            _stairs.Clear();
        }

        public override void StartEvaluation()
        {
            base.StartEvaluation();
            //Complete(true);

            
            CoroutineController.StartCoroutine(_evaluationRoutineKey, EvaluationRoutine());
        }

        private IEnumerator EvaluationRoutine()
        {
            yield return new WaitUntil(() => _player.Team.StickMen.All(s => s.IsStopped));

            var stair = GetStair();
            var multiplier = stair != null ? stair.Multiplier : 1;
            
            Debug.Log($"MultipliedBy: {multiplier}");
            stair.PlayRGB();
            Multiplied?.Invoke(multiplier);
            Complete(true);
        }

        private int GetStairsCount(int stickManCount)
        {
            return 1;
        }

        private Stair GetStair()
        {
            var stickManOnTop = _player.Team.StickMen
                .OrderByDescending(s => s.transform.position.y)
                .First();

            var ray = new Ray(stickManOnTop.transform.position + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _layerMask))
            {
                var stair = hit.collider.GetComponentInParent<Stair>();
                if (stair)
                {
                    return stair;
                }
            }

            return null;
        }
    }
}
