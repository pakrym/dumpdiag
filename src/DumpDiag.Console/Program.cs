using System.Reflection.PortableExecutable;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Diagnostics.Runtime.DacInterface;

namespace DumpDiag.Console
{
    public class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Option]
        public string DAC { get; set; }

        [Option]
        public string ProcessDump { get; set; }

        [Option(CommandOptionType.MultipleValue)]
        public string[] Analyzers { get; set; }

        private void OnExecute()
        {
            var context = AnalysisContext.FromProcessDump(ProcessDump, DAC);
            var analysisSession = new AnalysisSession(context);
            System.Console.CancelKeyPress += (sender, args) => analysisSession.Stop();

            AnalyzerFactory factory = new AnalyzerFactory();
            foreach (var analyzerName in Analyzers)
            {
                var analyzer = factory.CreateAnalyzer(analyzerName);
                analyzer.Run(analysisSession);
            }
        }
    }
}
