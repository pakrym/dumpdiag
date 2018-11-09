using System.Linq;

namespace DumpDiag.Console
{
    internal class DumpHeapAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            foreach (var o in analysisSession.Context.Runtime.Heap.EnumerateObjects().Take(50))
            {
                if (o.Type.IsString)
                {
                    analysisSession.Reporter.Write(new StringInstance((string)o.Type.GetValue(o.Address)));
                }
                else
                {
                    analysisSession.Reporter.Write(new ObjectInstance(o));
                }
            }
        }
    }
}