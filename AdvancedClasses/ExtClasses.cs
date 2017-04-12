using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedClasses
{

    public static class List
    {
        public static bool AnySafe<T>(this ICollection<T> source)
        {
            if (source == null)
            {
                return false;
            }
                return source.Any();
        }

        public static List<T> ToListSafe<T>(this ICollection<T> source)
        {
            if (!source.AnySafe())
            {
                return new List<T>();
            }
                return source.ToList();

        }
    }

}
