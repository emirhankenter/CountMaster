using System;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class LevelBehaviour : MonoBehaviour
    {
        public Action Started;
        public Action<bool, float> Completed;

        private PlayerController _playerController;

        [SerializeField] private StageBehaviour _finalStage;

        [SerializeField] private List<EnemyTeam> _enemyTeams;

        public PlayerController Player => _playerController;

        public int Multiplier { get; private set; }

        public void Initialize(PlayerController playerController)
        {
            _playerController = playerController;
            
            Started?.Invoke();

            _playerController.Team.PassedFinishLine += OnPlayerPassedFinishLine;
            _playerController.Team.Failed += OnPlayerFailed;
        }

        public void Dispose()
        {
            _playerController.Team.PassedFinishLine -= OnPlayerPassedFinishLine;
            _playerController.Team.Failed -= OnPlayerFailed;
            
            _finalStage.Dispose();

            foreach (var enemies in _enemyTeams)
            {
                enemies.Dispose();
            }
        }

        private void OnPlayerPassedFinishLine()
        {
            if (_finalStage is StairsStage stairsStage)
            {
                var desiredStairsCount = _playerController.Team.GetStairsList().Count;
                stairsStage.SetData(_playerController, desiredStairsCount);
                stairsStage.Init();
                
                _playerController.Team.CreateTriangle();

                StairsStage.Multiplied += OnStairResultReturned;
            }
        }

        private void OnPlayerFailed()
        {
            Completed?.Invoke(false, 1);
        }

        private void OnStairResultReturned(float multiplier)
        {
            StairsStage.Multiplied -= OnStairResultReturned;
            
            Completed?.Invoke(true, multiplier);
        }
    }
}
