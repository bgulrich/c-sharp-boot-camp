using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CollectionTests
{
    public static class Yields
    {
        public static IEnumerable<int> CountTo(int input)
        {
            var i = 0;

            while (true)
            {
                if (i < input)
                    yield return ++i;
                else
                    yield break;
            }
        }


        public static IEnumerable<string> GetLines(string filePath)
        {
            // read lines one at a time
            using (var textFile = File.OpenText(filePath))
            {
                string line;

                while ((line = textFile.ReadLine()) != null)
                {                  
                    yield return line;
                }
            }
        }
    }
}
