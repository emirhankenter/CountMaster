using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Remote
{
    public class RemoteInt : RemoteVariable<int>
    {
        public RemoteInt(string key, int defaultValue) : base(key, defaultValue)
        {
        }

        protected override void OnRemoteInitialized()
        {
            Value = RemoteConfig.GetInt(Key, Value);
        }
        
        public override string ToString() => $"{Value}";
        public static explicit operator int(RemoteInt r) => r.Value;
    }
}
