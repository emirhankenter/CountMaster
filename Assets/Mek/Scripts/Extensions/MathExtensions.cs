using System.Collections.Generic;
using UnityEngine;

namespace Mek.Extensions
{
    public static class MathExtensions
    {
        public static float Normalize(this float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public static float Denormalize(this float value, float min, float max)
        {
            return value * (max - min) + min;
        }

        public static int Avarage(this IList<int> list)
        {
            float sum = 0;

            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }

            return Mathf.RoundToInt(sum / list.Count);
        }

        public static float Avarage(this IList<float> list)
        {
            float sum = 0f;

            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }

            return sum / list.Count;
        }

        public static double Avarage(this IList<double> list)
        {
            double sum = 0f;

            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }

            return sum / list.Count;
        }
    }
}
