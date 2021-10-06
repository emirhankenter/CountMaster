using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class HammerBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _anchor;

        private Sequence _sequence;
        private void Start()
        {
            _sequence = DOTween.Sequence();

            _sequence.Append(
                _anchor.DOLocalRotate(new Vector3(0,0,90f), 0.5f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.InExpo)
            );
            
            _sequence.Append(
                _anchor.DOLocalRotate(new Vector3(0,0,-90f), 1f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.InExpo)
            );

            _sequence.SetLoops(-1, LoopType.Restart);
            _sequence.Play();
        }

        private void OnDestroy()
        {
            
        }
    }
}
