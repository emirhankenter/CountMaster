using Game.Scripts.Behaviours;
using Mek.Utilities;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class AssetController : SingletonBehaviour<AssetController>
    {
        [SerializeField] private StickMan _stickManPrefab;
        
        public StickMan StickManPrefab => _stickManPrefab;
        
        
        [SerializeField] private ExaminationBlock _examinationBlockPrefab;
        
        public ExaminationBlock ExaminationBlockPrefab => _examinationBlockPrefab;
        protected override void OnAwake()
        {
        }
    }
}
