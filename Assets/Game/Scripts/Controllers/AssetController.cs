using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Mek.Extensions;
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

        public List<Color> StairColors = new List<Color>();

        public Stair StairPrefab;
        protected override void OnAwake()
        {
        }
    }
}
