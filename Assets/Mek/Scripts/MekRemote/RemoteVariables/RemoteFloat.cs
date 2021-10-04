using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Remote
{
    public class RemoteFloat : RemoteVariable<float>
    {
        public RemoteFloat(string key, float defaultValue) : base(key, defaultValue)
        {
        }

        protected override void OnRemoteInitialized()
        {
            Value = RemoteConfig.GetFloat(Key, Value);
        }
        
        public override string ToString() => $"{Value}";
        public static explicit operator float(RemoteFloat r) => r.Value;
    }
}