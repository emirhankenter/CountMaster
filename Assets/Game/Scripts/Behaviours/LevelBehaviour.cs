using System;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class LevelBehaviour : MonoBehaviour
    {
        public Action Started;
        public Action<bool> Completed;

        private PlayerController _playerController;

        [SerializeField] private StageBehaviour _finalStage;

        public PlayerController Player => _playerController;

        public int Multiplier { get; private set; }

        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
            
            Started?.Invoke();

            _playerController.Team.Finished += OnPlayerFinished;
        }

        public void Dispose()
        {
        }

        private void OnPlayerPassedFinishLine()
        {
            if (_finalStage is StairsStage stairsStage)
            {
                var desiredStairsCount = _playerController.Team.GetStairsList().Count;
                stairsStage.Init();
                stairsStage.SetDesiredStairs(desiredStairsCount);
            }
        }

        private void OnStairResultReturned(float multiplier)
        {
        }

        private void OnPlayerFinished(bool isSuccess)
        {
            if (isSuccess)
            {
                OnPlayerPassedFinishLine();
            }
            else
            {
                Completed?.Invoke(false);
            }
        }
    }
}
