namespace DumpDiag.Console
{
    internal class ModulesAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            analysisSession.Reporter.Table("Modules", analysisSession.Context.Runtime.Modules.ToTable(
                "Name", "Informational version",
                module => module.Name, module => module.CreateMetadataReader().GetAssemblyAttributeStringValue("AssemblyInformationalVersionAttribute")));
        }
    }
}