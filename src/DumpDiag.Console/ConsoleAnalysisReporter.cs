using System;
using System.Collections.Generic;

namespace DumpDiag.Console
{
    internal class ConsoleAnalysisReporter : IAnalysisReporter
    {
        public void Info(string message)
        {
            System.Console.WriteLine(message);
        }

        public void Table(string name, IEnumerable<IEnumerable<string>> table)
        {
            System.Console.WriteLine("--------------- " + name);
            var columnSizes = new List<int>();
            foreach (var row in table)
            {
                int column = 0;
                foreach (var rowValue in row)
                {
                    var value = rowValue ?? string.Empty;
                    if (columnSizes.Count <= column)
                    {
                        columnSizes.Add(value.Length);
                    }
                    else
                    {
                        columnSizes[column] = Math.Max(columnSizes[column], value.Length);
                    }
                    column++;
                }
            }

            foreach (var row in table)
            {
                var column = 0;
                foreach (var value in row)
                {
                    System.Console.Write(value);
                    var count = columnSizes[column] - value.Length + 1;
                    System.Console.Write(new string(' ', count));
                    column++;
                }

                System.Console.WriteLine();
            }
        }

        public void Write(object o)
        {
        }
    }
}