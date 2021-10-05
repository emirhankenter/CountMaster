using Game.Scripts.Behaviours;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class AssetController : SingletonBehaviour<AssetController>
    {
        [SerializeField] private StickMan _stickManPrefab;
        
        public StickMan StickManPrefab => _stickManPrefab;
        protected override void OnAwake()
        {
        }
    }
}
