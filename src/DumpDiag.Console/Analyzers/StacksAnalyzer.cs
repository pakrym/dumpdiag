using DumpDiag.Console.Models;

namespace DumpDiag.Console.Analyzers
{
    public class StacksAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession, string[] arguments)
        {
            foreach (var runtimeThread in analysisSession.Context.Runtime.Threads)
            {
                analysisSession.Reporter.Write(new ThreadStacks(runtimeThread));
            }
        }
    }
}