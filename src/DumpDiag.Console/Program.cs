using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using DumpDiag.Web;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Diagnostics.Runtime.DacInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        [Option]
        public  bool Web { get; set; }

        [Option(CommandOptionType.MultipleValue)]
        public string[] Analyzers { get; set; } = Array.Empty<string>();

        private void OnExecute()
        {
            var context = AnalysisContext.FromProcessDump(ProcessDump, DAC);

            AnalyzerFactory factory = new AnalyzerFactory();

            if (Web)
            {
                new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureServices(
                        collection => {
                            collection.AddSingleton<AnalyzerFactory>(factory);
                            collection.AddSingleton<AnalysisContext>(context);
                        })
                    .UseStartup<Startup>().Build().Run();
            }
            else
            {
                var reporter = new ConsoleAnalysisReporter();
                var analysisSession = new AnalysisSession(context, reporter);
                System.Console.CancelKeyPress += (sender, args) => analysisSession.Stop();
                foreach (var analyzerName in Analyzers)
                {
                    var analyzer = factory.CreateAnalyzer(analyzerName);
                    analyzer.Run(analysisSession, new string[] {});
                }
            }
        }
    }
}
