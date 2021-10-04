using System;
using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Controllers
{
    public class MekGM : SingletonBehaviour<MekGM>
    {
        public static event Action Initialized; 
        public bool IsInitialized { get; private set; }
        protected override void OnAwake()
        {
            RemoteConfig.Initialize();
            RemoteConfig.DoAfterInitialized(() =>
            {
                IsInitialized = true;
                Initialized?.Invoke();
                OnInitialized();
            });
        }

        private void OnInitialized()
        {
            Debug.Log("MekGM has initialized!");
        }
        
        public void DoAfterInitialized(Action callback)
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
    }
}