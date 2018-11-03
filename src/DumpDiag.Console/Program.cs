using System;
using System.Collections.Generic;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    public class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [Option]
        public string DAC { get; set;  }

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

    internal class ModulesAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            analysisSession.Reporter.Table("Modules", analysisSession.Context.Runtime.Modules.ToTable(
                "Name", "Dll",
                module => module.Name, module => module.FileName));
        }
    }

    internal class AnalysisSession
    {
        public AnalysisContext Context { get; }

        private readonly CancellationTokenSource _cancellationTokenSource;

        public AnalysisSession(AnalysisContext context)
        {
            Context = context;
            Reporter = new ConsoleAnalysisReporter();
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken = _cancellationTokenSource.Token;
        }

        public IAnalysisReporter Reporter { get; }
        public CancellationToken CancellationToken { get;  }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }

    internal class ConsoleAnalysisReporter : IAnalysisReporter
    {
        public void Info(string message)
        {
            System.Console.WriteLine(message);
        }

        public void Progress(string progressMessage)
        {
            System.Console.WriteLine(progressMessage);
        }

        public void Table(string name, IEnumerable<IEnumerable<string>> table)
        {
            System.Console.WriteLine("--------------- " + name);
            var columnSizes = new List<int>();
            foreach (var row in table)
            {
                int column = 0;
                foreach (var rowValue in row)
                {
                    var value = rowValue ?? string.Empty;
                    if (columnSizes.Count <= column)
                    {
                        columnSizes.Add(value.Length);
                    }
                    else
                    {
                        columnSizes[column] = Math.Max(columnSizes[column], value.Length);
                    }
                    column++;
                }
            }

            foreach (var row in table)
            {
                var column = 0;
                foreach (var value in row)
                {
                    System.Console.Write(value);
                    var count = columnSizes[column] - value.Length + 1;
                    System.Console.Write(new string(' ', count));
                }

                System.Console.WriteLine();
            }
        }
    }

    interface IAnalysisReporter
    {
        void Info(string message);
        void Progress(string progressMessage);
        void Table(string name, IEnumerable<IEnumerable<string>> table);
    }

    internal class AnalyzerFactory
    {
        private readonly List<Type> _analyzers = new List<Type>();

        public AnalyzerFactory()
        {
            var assemblies = new []
            {
                GetType().Assembly
            };

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsAbstract && typeof(IDumpAnalyzer).IsAssignableFrom(type))
                    {
                        _analyzers.Add(type);
                    }
                }
            }
        }

        public IDumpAnalyzer CreateAnalyzer(string name)
        {
            foreach (var analyzer in _analyzers)
            {
                if (analyzer.Name.Contains(name))
                {
                    return (IDumpAnalyzer)Activator.CreateInstance(analyzer);
                }
            }

            throw new InvalidOperationException($"Analyzer {name} not found. Available: {string.Join(",", _analyzers)}");
        }
    }

    internal class AnalysisContext
    {
        public DataTarget DataTarget { get; }
        public ClrRuntime Runtime { get; }

        private AnalysisContext(DataTarget dataTarget, ClrRuntime runtime)
        {
            DataTarget = dataTarget;
            Runtime = runtime;
        }

        public static AnalysisContext FromProcessDump(string processDump, string dacPath)
        {
            var dataTarget = DataTarget.LoadCrashDump(processDump);
            ClrInfo runtimeInfo = dataTarget.ClrVersions[0];  // just using the first runtime
            ClrRuntime runtime = runtimeInfo.CreateRuntime(dacPath);
            return new AnalysisContext(dataTarget, runtime);
        }
    }
}
