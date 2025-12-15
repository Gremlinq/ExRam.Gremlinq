using System;
using System.Collections.Generic;
using System.Text;

namespace ExRam.Gremlinq.Templates.Console
{
    public static class Extensions
    {
#if (true) // --8<-- [start:writeToConsole]
        public static void WriteToConsole(this Airport[] airports, string caption)
        {
            Console
                .WriteLine($"{caption}:");

            foreach (var airport in airports)
            {
                Console
                    .WriteLine($"  {airport.Code} ({airport.Description})");
            }

            Console
                .WriteLine();
        }
#endif  // --8<-- [end:writeToConsole]
    }
}
