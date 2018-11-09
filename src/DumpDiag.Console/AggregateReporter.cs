using System.Collections.Generic;

namespace DumpDiag.Console
{
    internal class AggregateReporter: IAnalysisReporter
    {
        public List<object> Results { get; } = new List<object>();

        public void Write(object o)
        {
            Results.Add(o);
        }
    }
}
