using System;
using Game.Scripts.Enums;
using Game.Scripts.Models;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class ExaminationBlock : MonoBehaviour
    {
        public event Action<ExaminationData> TriggerEntered;
        
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshPro _text;
        
        public ExaminationData Data { get; private set; }

        public void Initialize(ExaminationData data)
        {
            Data = data;
            
            var color = Data.Operation == MathOperation.Addition || Data.Operation == MathOperation.Multiplication
                ? GameConfig.StickManColorFriendly
                : GameConfig.StickManColorEnemy;
            color.a = 0.8f;
            _renderer.material.color = color;
            
            SetText();
        }

        private void SetText()
        {
            _text.SetText(GetPrefix() + Data.Value);
        }

        private string GetPrefix()
        {
            switch (Data.Operation)
            {
                case MathOperation.Addition:
                    return "+";
                case MathOperation.Subtraction:
                    return "-";
                case MathOperation.Multiplication:
                    return "x";
                case MathOperation.Division:
                    return "÷";
            }

            return "ERROR";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("StickMan")) return;
            TriggerEntered?.Invoke(Data);
        }
    }
}
