using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Game.Scripts.Models;
using Game.Scripts.View;
using Mek.Controllers;
using Mek.Coroutines;
using Mek.Localization;
using Mek.Models;
using Mek.Navigation;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : SingletonBehaviour<GameController>
    {
        [SerializeField] private PlayerController _playerController;
        
        [SerializeField] private List<LevelBehaviour> _levels;
        
        public LevelBehaviour CurrentLevel { get; private set; }
        
        protected override void OnAwake()
        {
            LocalizationManager.Init(SystemLanguage.Turkish);
            PrefsManager.Initialize(PlayerData.Prefs);
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Ready");
            });
            
            PrepareGame();
        }

        private void PrepareGame()
        {
            CurrentLevel = Instantiate(_levels[(PlayerData.Instance.PlayerLevel - 1) % _levels.Count]);
            
            CurrentLevel.Initialize(_playerController);

            CurrentLevel.Completed += OnLevelCompleted;
            
            _playerController.Initialize();

            Navigation.Panel.Change(ViewTypes.InGamePanel);
        }

        private void DisposeLevel()
        {
            CurrentLevel.Completed -= OnLevelCompleted;
            CurrentLevel.Dispose();
            
            _playerController.Dispose();
            
            Destroy(CurrentLevel.gameObject, Time.fixedDeltaTime);
        }
        private void OnLevelCompleted(bool state, float multiplier)
        {
            Debug.Log($"LevelCompleted: {state}");

            var reward = Mathf.RoundToInt(PlayerData.Instance.PlayerLevel * 10 * multiplier);

            Navigation.Panel.Change(new EndGamePanelParameters(reward, OnRewardClaimed));

            PlayerData.Instance.Coin += reward;
            PlayerData.Instance.PlayerLevel++;
        }

        private void OnRewardClaimed()
        {
            DisposeLevel();
            
            CoroutineController.DoAfterFixedUpdate(PrepareGame);
        }
    }
}
