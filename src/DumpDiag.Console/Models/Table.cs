using System.Collections.Generic;

namespace DumpDiag.Console.Models
{
    public class Table
    {
        public Table(string name, IEnumerable<IEnumerable<string>> data)
        {
            Name = name;
            Data = data;
        }

        public IEnumerable<IEnumerable<string>> Data { get; set; }
        public string Name { get; set; }
    }
}