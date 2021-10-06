using System;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class EnemyTeam : Team
    {
        public override TeamSide TeamSide => TeamSide.Enemy;

        [SerializeField] private int _count = 10;
        
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            
            Initialize(_count);
        }

        public override void Initialize(int count)
        {
            base.Initialize(count);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override void OnStickManLost(StickMan stickMan)
        {
            base.OnStickManLost(stickMan);
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private SphereCollider _sphereCollider;
        private void OnDrawGizmos()
        {
            if (_sphereCollider == null)
            {
                _sphereCollider = GetComponent<SphereCollider>();
            }
            
            Gizmos.color = new Color(1,0,0,0.5f);
            Gizmos.DrawSphere(_sphereCollider.bounds.center, _sphereCollider.radius);
        }
#endif
    }
}
