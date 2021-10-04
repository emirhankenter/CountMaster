using System;
using Mek.Extensions;
using UnityEngine;

namespace Mek.Utilities
{
    public class PooledParticleObject : MonoBehaviour
    {
        private ParticleSystem _particle;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        private void OnParticleSystemStopped()
        {
            _particle.Recycle();
        }
    }
}
