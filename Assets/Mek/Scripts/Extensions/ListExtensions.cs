using System.Collections.Generic;

namespace Mek.Extensions
{
    public static class ListExtensions
    {
        private static System.Random Random = new System.Random();

        public static T RandomElement<T>(this IList<T> list)
        {
            var index = Random.Next(0, list.Count);
            return list[index];
        }
        public static T First<T>(this IList<T> list)
        {
            return list[0];
        }

        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }
    }
}
