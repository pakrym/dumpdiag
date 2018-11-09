using DumpDiag.Web;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace DumpDiag.Console
{
    public class Program
    {
        public static int Main(string[] args)
        {
            DebugHelper.HandleDebugSwitch(ref args);
            return CommandLineApplication.Execute<Program>(args);
        }

        [Option]
        public string DAC { get; set; }

        [Option]
        public string ProcessDump { get; set; }

        [Option]
        public bool Web { get; set; }

        [Option(CommandOptionType.MultipleValue)]
        public string[] Analyzers { get; set; } = Array.Empty<string>();

        private void OnExecute()
        {
            if (Web)
            {
                new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>().Build().Run();
            }
            else
            {
                var context = AnalysisContext.FromProcessDump(ProcessDump, DAC);
                var analysisSession = new AnalysisSession(context);

                System.Console.CancelKeyPress += (sender, args) => analysisSession.Stop();

                if (context.DataTarget != null)
                {
                    var factory = new AnalyzerFactory();
                    foreach (var analyzerName in Analyzers)
                    {
                        var analyzer = factory.CreateAnalyzer(analyzerName);
                        analyzer.Run(analysisSession);
                    }
                }
            }
        }
    }
}
