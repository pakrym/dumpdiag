using System.Collections.Generic;

namespace DumpDiag.Console.Models
{
    public class Result
    {
        public Result(string command, IEnumerable<object> results)
        {
            Command = command;
            Results = results;
        }

        public string Command { get; }
        public IEnumerable<object> Results {get;}
    }
}