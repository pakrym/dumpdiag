namespace DumpDiag.Console
{
    internal class InitAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            analysisSession.Reporter.Write(new FormattedString("Welcome. Try executing ", new CommandRef("modules"), " or ", new CommandRef("threadpool"), " command"));
        }
    }
}