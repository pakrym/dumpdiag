using System;
using System.Linq;

namespace DumpDiag.Console
{
    internal class DumpHeapAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession, string[] arguments)
        {
            foreach (var o in analysisSession.Context.Runtime.Heap.EnumerateObjects().Take(50))
            {
                analysisSession.Reporter.Write(o.Describe());
            }
        }
    }
}