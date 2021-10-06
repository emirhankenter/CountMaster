using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Behaviours
{
    public class Stair : MonoBehaviour
    {
        public const float Height = 1.8f;
        public const float Lenght = 4f;
        
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshPro _multiplierText;

        private Color _color;
        public float Multiplier { get; private set; }

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
        
    }
}
