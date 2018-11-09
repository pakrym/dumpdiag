using System.Collections.Generic;

namespace DumpDiag.Console
{
    public interface IAnalysisReporter
    {
        void Table(string name, IEnumerable<IEnumerable<string>> table);
    }
}