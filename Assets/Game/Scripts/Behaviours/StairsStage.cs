using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Controllers;
using Mek.Coroutines;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class StairsStage : StageBehaviour
    {
        public static event Action<float> Multiplied;

        private PlayerController _player;

        private string _evaluationRoutineKey => $"evaluationRoutine{GetInstanceID()}";

        private float _stairsCount;
        
        public void SetDesiredStairs(int count)
        {
            _stairsCount = count;
        }

        public override void Init()
        {
            // _player = GameController.Instance.CurrentLevel.Player;
            base.Init();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (CoroutineController.IsCoroutineRunning(_evaluationRoutineKey))
            {
                CoroutineController.StopCoroutine(_evaluationRoutineKey);
            }
        }

        public override void StartEvaluation()
        {
            base.StartEvaluation();
            //Complete(true);

            var stairsCount = GetStairsCount(_player.Team.StickMen.Count);
            
            CoroutineController.StartCoroutine(_evaluationRoutineKey, EvaluationRoutine());
        }

        private IEnumerator EvaluationRoutine()
        {
            yield return new WaitUntil(() => !_player.Team.IsRunning);

            var stair = GetStair();
            var multiplier = stair != null ? stair.Multiplier : 1;
            Debug.Log($"MultipliedBy: {multiplier}");
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

            var ray = new Ray(stickManOnTop.transform.position, Vector3.down);

            if (Physics.Raycast(ray, out var hit))
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
