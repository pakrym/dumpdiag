using System.Threading;

namespace DumpDiag.Console
{
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
}