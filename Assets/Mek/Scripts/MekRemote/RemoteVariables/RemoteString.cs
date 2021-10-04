using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Remote
{
    public class RemoteString : RemoteVariable<string>
    {
        public RemoteString(string key, string defaultValue) : base(key, defaultValue)
        {
        }

        protected override void OnRemoteInitialized()
        {
            Value = RemoteConfig.GetString(Key, Value);
        }
        
        public override string ToString() => $"{Value}";
    }
}