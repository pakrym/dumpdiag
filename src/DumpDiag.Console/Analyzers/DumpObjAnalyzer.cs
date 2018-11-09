using System.Globalization;
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
}