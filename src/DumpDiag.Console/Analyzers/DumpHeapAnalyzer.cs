using System;
using System.Globalization;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    internal class DumpObjAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession, string[] arguments)
        {
            var address = ulong.Parse(arguments[0], NumberStyles.HexNumber);
            var type = analysisSession.Context.Runtime.Heap.GetObjectType(address);
            analysisSession.Reporter.Write(new ClrObject(address, type).Describe());
        }
    }

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