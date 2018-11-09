using System;

namespace DumpDiag.Console
{
    public interface IDumpAnalyzer
    {
        void Run(AnalysisSession analysisSession, string[] arguments);
    }
}