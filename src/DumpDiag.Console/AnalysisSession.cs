using System.Threading;

namespace DumpDiag.Console
{
    public class AnalysisSession
    {
        public AnalysisContext Context { get; }

        private readonly CancellationTokenSource _cancellationTokenSource;

        public AnalysisSession(AnalysisContext context, IAnalysisReporter reporter)
        {
            Context = context;
            Reporter = reporter;
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