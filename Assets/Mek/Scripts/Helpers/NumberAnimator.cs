using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Mek.Helpers
{
    public class NumberAnimator
    {
        protected float Current;
        protected Text Text;
        public NumberAnimator(float current, Text text)
        {
            Text = text;
            SetCurrent(current);
        }

        public void SetCurrent(float current, bool roundToInt = false)
        {
            Current = current;

            if (roundToInt)
            {
                Current = Mathf.RoundToInt(Current);
            }
            Text.text = Current.ToString();
        }

        public void UpdateValue(float to, float duration = 0.5f, bool roundToInt = false)
        {
            DOTween.To(
                getter: () => Current,
                setter: current => SetCurrent(current, roundToInt),
                endValue: to,
                duration);
        }
    }
}