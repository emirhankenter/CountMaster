using GameAnalyticsSDK;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Scripts.Controllers
{
    public class AnalyticsController : SingletonBehaviour<AnalyticsController>, IGameAnalyticsATTListener 
    {
        protected override void OnAwake()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                GameAnalytics.Initialize();
            }
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            GameAnalytics.Initialize();
        }
        public void GameAnalyticsATTListenerRestricted()
        {
            GameAnalytics.Initialize();
        }
        public void GameAnalyticsATTListenerDenied()
        {
            GameAnalytics.Initialize();
        }
        public void GameAnalyticsATTListenerAuthorized()
        {
            GameAnalytics.Initialize();
        }

        public void LogEvent(GAErrorSeverity severity, string message)
        {
            GameAnalytics.NewErrorEvent(severity, message);
        }
    }
}