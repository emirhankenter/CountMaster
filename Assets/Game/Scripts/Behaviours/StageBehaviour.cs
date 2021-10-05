using System;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public abstract class StageBehaviour : MonoBehaviour
    {
        public event Action<bool> Result;
        
        public virtual void Init()
        {
        }

        public virtual void Dispose()
        {

        }

        public virtual void StartEvaluation()
        {

        }

        protected virtual void Complete(bool state)
        {
            Result?.Invoke(state);
        }
    }
}