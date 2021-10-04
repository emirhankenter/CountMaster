using Mek.Remote;
using Mek.Utilities;
using UnityEngine;

namespace Mek.Remote
{
    public class RemoteLong : RemoteVariable<long>
    {
        public RemoteLong(string key, long defaultValue) : base(key, defaultValue)
        {
        }

        protected override void OnRemoteInitialized()
        {
            Value = RemoteConfig.GetLong(Key, Value);
        }
        
        public override string ToString() => $"{Value}";
        public static explicit operator long(RemoteLong r) => r.Value;
    }
}