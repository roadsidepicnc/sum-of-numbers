using System.Collections.Generic;
using Random = System.Random;

namespace Utilities
{
    public static class ListExtensions
    {
        private static Random _random = new Random();
        
        public static void RefreshWith<T>(this List<T> list, IEnumerable<T> items)
        {
            list.Clear();
            list.AddRange(items);
        }
        
        public static void Shuffle<T>(this IList<T> list)
        { 
            _random = new Random();
            var i = list.Count;
            
            while (i > 1)
            {
                i--;
                var j = _random.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}