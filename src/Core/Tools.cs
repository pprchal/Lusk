using System.Collections.Generic;
using System.IO;

namespace Lusk.Core
{
    internal static class Tools
    {
        public static IEnumerable<string> AsLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length == 0)
                {
                    break;
                }
                yield return line;
            }
        }
    }
}
