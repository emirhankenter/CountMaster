using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Remote
{
    public class RemoteVariable<T>
    {
        public string Key;
        public T Value;
        
        public RemoteVariable(string key, T defaultValue)
        {
            Key = key;
            Value = defaultValue;
            RemoteConfig.DoAfterInitialized(OnRemoteInitialized);
        }

        protected virtual void OnRemoteInitialized()
        {
        }
    }
}
