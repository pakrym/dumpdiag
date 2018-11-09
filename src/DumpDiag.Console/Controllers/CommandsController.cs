using System;
using Microsoft.AspNetCore.Mvc;

namespace DumpDiag.Console.Controllers
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

        public IActionResult Index(string command, string arguments)
        {
            var analyzer = _analyzerFactory.CreateAnalyzer(command);
            var reporter = new AggregateReporter();
            var session = new AnalysisSession(_context, reporter);
            analyzer.Run(session, (arguments ?? string.Empty).Split(" "));
            return View("Result", reporter.Results);
        }
    }
}
