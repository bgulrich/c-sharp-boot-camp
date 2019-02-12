using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Generics
{
    public static class Extensions
    {
        public static IEnumerable<TOut> ToEnumerableOf<TIn, TOut>(this IEnumerable<TIn> input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(TIn));
            var outputType = typeof(TOut);

            foreach(var item in input)
            {
                yield return (TOut)converter.ConvertTo(item, outputType);
            }
        }    
    }   
}
