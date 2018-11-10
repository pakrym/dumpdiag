using System;
using System.Linq;

namespace DumpDiag.Console
{
    internal class DumpHeapAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession, string[] arguments)
        {
            var clrObjects = analysisSession.Context.Runtime.Heap.EnumerateObjects();
            var typeName = arguments.FirstOrDefault();
            if (!string.IsNullOrEmpty(typeName))
            {
                clrObjects = clrObjects.Where(t => t.Type?.Name == typeName);
            }
            foreach (var o in clrObjects.Take(50))
            {
                analysisSession.Reporter.Write(o.Describe());
            }
        }
    }
}