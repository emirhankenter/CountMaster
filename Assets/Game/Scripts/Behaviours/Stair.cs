using System;
using DG.Tweening;
using Mek.Interfaces;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Behaviours
{
    public class Stair : MonoBehaviour, IRecycleCallbackReceiver
    {
        public const float Height = 1.8f;
        public const float Lenght = 4f;
        
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshPro _multiplierText;

        private Color _color;
        public float Multiplier { get; private set; }

        private Sequence _sequence;

        public void Initialize(float multiplier, Color color)
        {
            Multiplier = multiplier;
            _color = color;
            
            UpdateColorAndText();
        }

        private void UpdateColorAndText()
        {
            _renderer.material.color = _color;
            _multiplierText.SetText($"x{Multiplier}");
        }

        public void PlayRGB()
        {
            _renderer.material.color = Color.white;

            _sequence = DOTween.Sequence();

            _sequence.Append(
                _renderer.material.DOColor(Color.red, 0.2f)
                    .SetEase(Ease.Linear)
            );
            _sequence.Append(
                _renderer.material.DOColor(Color.green, 0.2f)
                    .SetEase(Ease.Linear)
            );
            _sequence.Append(
                _renderer.material.DOColor(Color.blue, 0.2f)
                    .SetEase(Ease.Linear)
            );

            _sequence.SetLoops(-1, LoopType.Yoyo);

            _sequence.Play();
        }

        public void OnRecycle()
        {
            _sequence?.Kill();
        }
    }
}
