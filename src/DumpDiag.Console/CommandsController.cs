using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DumpDiag.Console.Pages
{
    public class CommandsController: Controller
    {
        private readonly AnalysisContext _context;

        private readonly AnalyzerFactory _analyzerFactory;

        public CommandsController(AnalysisContext context, AnalyzerFactory analyzerFactory)
        {
            _context = context;
            _analyzerFactory = analyzerFactory;
        }

        public IActionResult Index(string name)
        {
            var analyzer = _analyzerFactory.CreateAnalyzer(name);
            var reporter = new AggregateReporter();
            var session = new AnalysisSession(_context, reporter);
            analyzer.Run(session);
            return View("Result", reporter.Results);
        }
    }

    internal class AggregateReporter: IAnalysisReporter
    {
        public List<object> Results { get; } = new List<object>();

        public void Info(string message)
        {
            Results.Add(message);
        }

        public void Table(string name, IEnumerable<IEnumerable<string>> table)
        {
            Results.Add(new Table()
            {
                Name =  name,
                Data = table
            });
        }

        public void Write(object o)
        {
            Results.Add(o);
        }
    }
}
