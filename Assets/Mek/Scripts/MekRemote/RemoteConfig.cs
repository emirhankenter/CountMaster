using System;
using Unity.RemoteConfig;
using UnityEngine;

namespace Mek.Remote
{
    public static class RemoteConfig
    {
        public static event Action Initialized; 
        private struct _userAttribures {}
        private struct _appAttribures {}

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            ConfigManager.FetchCompleted += OnFetchCompleted;
            ConfigManager.FetchConfigs(new _userAttribures(), new _appAttribures());
        }

        private static void OnFetchCompleted(ConfigResponse obj)
        {
            ConfigManager.FetchCompleted -= OnFetchCompleted;

            IsInitialized = true;
            Initialized?.Invoke();
            
            Debug.Log("RemoteConfig has initialized!");
        }

        #region Utils

        public static void DoAfterInitialized(Action callback)
        {
            if (IsInitialized)
            {
                callback?.Invoke();
            }
            else
            {
                Initialized += InternalOnInitialized;

                void InternalOnInitialized()
                {
                    Initialized -= InternalOnInitialized;
                    callback?.Invoke();
                }
            }
        }

        public static bool HasKey(string key)
        {
            return ConfigManager.appConfig.HasKey(key);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            if (!HasKey(key) || !IsInitialized)
            {
                return defaultValue;
            }
            return ConfigManager.appConfig.GetBool(key, defaultValue);
        }
        
        public static long GetLong(string key, long defaultValue)
        {
            if (!HasKey(key) || !IsInitialized)
            {
                return defaultValue;
            }
            return ConfigManager.appConfig.GetLong(key, defaultValue);
        }
        
        public static int GetInt(string key, int defaultValue)
        {
            if (!HasKey(key) || !IsInitialized)
            {
                return defaultValue;
            }
            return ConfigManager.appConfig.GetInt(key, defaultValue);
        }
        
        public static float GetFloat(string key, float defaultValue)
        {
            if (!HasKey(key) || !IsInitialized)
            {
                return defaultValue;
            }
            return ConfigManager.appConfig.GetFloat(key, defaultValue);
        }
        
        public static string GetString(string key, string defaultValue)
        {
            if (!HasKey(key) || !IsInitialized)
            {
                return defaultValue;
            }
            return ConfigManager.appConfig.GetString(key, defaultValue);
        }

        public static string[] GetKeys()
        {
            if (!IsInitialized)
            {
                return null;
            }
            return ConfigManager.appConfig.GetKeys();
        }

        #endregion
    }
}
