using System.Collections.Generic;

namespace Coinche.Server.Utils
{
    /// <summary>
    /// Shift list.
    /// </summary>
    public static class ShiftList
    {
        /// <summary>
        /// Left shift a list.
        /// </summary>
        /// <returns>The left.</returns>
        /// <param name="list">List.</param>
        /// <param name="shiftBy">Shift by.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
            {
                return list;
            }

            var result = list.GetRange(shiftBy, list.Count - shiftBy);
            result.AddRange(list.GetRange(0, shiftBy));
            return result;
        }

        /// <summary>
        /// Right shift a list.
        /// </summary>
        /// <returns>The right.</returns>
        /// <param name="list">List.</param>
        /// <param name="shiftBy">Shift by.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ShiftRight<T>(this List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
            {
                return list;
            }

            var result = list.GetRange(list.Count - shiftBy, shiftBy);
            result.AddRange(list.GetRange(0, list.Count - shiftBy));
            return result;
        }
    }
}
