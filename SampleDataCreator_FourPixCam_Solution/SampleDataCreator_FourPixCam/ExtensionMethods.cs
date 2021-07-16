using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleDataCreator_FourPixCam
{
    public static class ExtensionMethods
    {
        internal static List<T> ToList<T>(this Array arr)
        {
            return arr.Cast<T>().ToList();
        }
    }
}
