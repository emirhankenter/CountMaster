using Mek.Models;
using Mek.Remote;
using UnityEngine;

namespace Game.Scripts.Models
{
    public static class GameConfig
    {
        public const float Bounds = 10f;
        public static Color StickManColorFriendly => new Color(0.2028747f, 0.4651468f, 0.9150943f, 1f);
        public static Color StickManColorEnemy => new Color(0.8867924f, 0.004182993f, 0.02769902f, 1f);
        
        public static RemoteInt IntRemoteExample = new RemoteInt("intTest", 100);
        public static RemoteString StringRemoteExample = new RemoteString("stringTest", "string");
        public static RemoteLong LongRemoteExample = new RemoteLong("longTest", 100);
        public static RemoteFloat FloatRemoteExample = new RemoteFloat("floatTest", 2.5f);
    }
}
