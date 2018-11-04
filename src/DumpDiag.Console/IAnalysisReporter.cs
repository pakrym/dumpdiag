using System.Collections.Generic;

namespace DumpDiag.Console
{
    interface IAnalysisReporter
    {
        void Info(string message);
        void Progress(string progressMessage);
        void Table(string name, IEnumerable<IEnumerable<string>> table);
    }
}