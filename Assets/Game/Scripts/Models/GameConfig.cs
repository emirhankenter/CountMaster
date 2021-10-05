using Mek.Models;
using Mek.Remote;
using UnityEngine;

namespace Game.Scripts.Models
{
    public static class GameConfig
    {
        public const float Bounds = 10f;
        
        public static RemoteInt IntRemoteExample = new RemoteInt("intTest", 100);
        public static RemoteJson<TestClass> JsonRemoteExample = new RemoteJson<TestClass>("jsonTest", JsonUtility.ToJson(new TestClass(){Number =  150}));
        public static RemoteString StringRemoteExample = new RemoteString("stringTest", "string");
        public static RemoteLong LongRemoteExample = new RemoteLong("longTest", 100);
        public static RemoteFloat FloatRemoteExample = new RemoteFloat("floatTest", 2.5f);
    }
}
