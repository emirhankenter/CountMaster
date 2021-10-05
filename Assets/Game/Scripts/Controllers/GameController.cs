using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Game.Scripts.Models;
using Mek.Controllers;
using Mek.Localization;
using Mek.Models;
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
        }

        private void DisposeLevel()
        {
            CurrentLevel.Completed -= OnLevelCompleted;
            CurrentLevel.Dispose();
            
            Destroy(CurrentLevel.gameObject);
        }
        private void OnLevelCompleted(bool state)
        {
            Debug.Log($"LevelCompleted: {state}");
        }
    }
}
