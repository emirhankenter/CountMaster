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
        protected override void OnAwake()
        {
            LocalizationManager.Init(SystemLanguage.Turkish);
            PrefsManager.Initialize(PlayerData.Prefs);
            
            MekGM.Instance.DoAfterInitialized(() =>
            {
                Debug.Log("Ready");
            });
        }
    }
}
