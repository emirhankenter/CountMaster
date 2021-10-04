using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Remote
{
    public class RemoteJson<T> : RemoteString
    {
        public RemoteJson(string key, string defaultValue) : base(key, defaultValue)
        {
        }

        protected override void OnRemoteInitialized()
        {
            Value = RemoteConfig.GetString(Key, Value);
        }
        
        public override string ToString() => $"{Value}";
        public static explicit operator T(RemoteJson<T> str) => JsonUtility.FromJson<T>(str.Value);
    }
}
