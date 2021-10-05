using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Controllers;
using Game.Scripts.Enums;
using Mek.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    [Serializable]
    public struct ExaminationData
    {
        public MathOperation Operation;
        public float Value;
    }
    public class ExaminationBlocks : MonoBehaviour
    {
        public static event Action<ExaminationData> ExaminationCompleted; 
        
        [Serializable] public class ExaminationBlockDictionary : SerializableDictionary<ExaminationBlock, ExaminationData>{}

        [SerializeField, OnValueChanged(nameof(OnBlockCollectionChanged))] private ExaminationBlockDictionary _dictionary;
        
        public bool HasComplete { get; private set; }
        
        private void Start()
        {
            foreach (var pair in _dictionary)
            {
                var examinationBlock = pair.Key;
                var data = pair.Value;
                examinationBlock.Initialize(data);

                pair.Key.TriggerEntered += OnExaminationComplete;
            }

            if (_dictionary.Count > 1)
            {
                _dictionary.First().Key.transform.localPosition = new Vector3(-4.5f, 0, 0);
                _dictionary.Last().Key.transform.localPosition = new Vector3(4.5f, 0, 0);
            }
        }

        private void OnExaminationComplete(ExaminationData data)
        {
            if (HasComplete) return;
            HasComplete = true;
            Dispose();
            ExaminationCompleted?.Invoke(data);
        }

        private void Dispose()
        {
            foreach (var pair in _dictionary)
            {
                pair.Key.TriggerEntered -= OnExaminationComplete;
            }
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        private void OnBlockCollectionChanged()
        {
            if (_dictionary.Count == 1)
            {
                _dictionary.First().Key.transform.localPosition = new Vector3(0, 0, 0);
            }
            else if (_dictionary.Count > 1)
            {
                _dictionary.First().Key.transform.localPosition = new Vector3(-4.5f, 0, 0);
                _dictionary.Last().Key.transform.localPosition = new Vector3(4.5f, 0, 0);
            }
        }
    }
}
