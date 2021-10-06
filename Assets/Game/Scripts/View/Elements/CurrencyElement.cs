using Game.Scripts.Models;
using Mek.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.Elements
{
    public class CurrencyElement : MonoBehaviour
    {
        [SerializeField] private Text _coinText;

        private NumberAnimator _coinNumberAnimator;
        
        public void Init(int current)
        {
            if (_coinNumberAnimator == null)
            {
                _coinNumberAnimator = new NumberAnimator(current, _coinText);
            }
            else
            {
                _coinNumberAnimator.SetCurrent(current);
            }
        }

        public void UpdateValue(float to)
        {
            _coinNumberAnimator.UpdateValue(to);
        }
    }
}
