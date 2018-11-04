namespace DumpDiag.Console
{
    internal class ThreadPoolAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            var pool = analysisSession.Context.Runtime.ThreadPool;
            analysisSession.Reporter.Table("ThreadPool status",
                new[]
                {
                    new [] {nameof(pool.TotalThreads), pool.TotalThreads.ToString() },
                    new [] {nameof(pool.RunningThreads), pool.RunningThreads.ToString() },
                    new [] {nameof(pool.IdleThreads), pool.IdleThreads.ToString() },
                    new [] {nameof(pool.CpuUtilization), pool.CpuUtilization.ToString() },
                    new [] {nameof(pool.FreeCompletionPortCount), pool.FreeCompletionPortCount.ToString() }
                });

            analysisSession.Reporter.Table("ThreadPool limits",
                new[]
                {
                    new [] { nameof(pool.MinThreads), pool.MinThreads.ToString() },
                    new [] { nameof(pool.MaxThreads), pool.MaxThreads.ToString() },
                    new [] { nameof(pool.MinCompletionPorts), pool.MinCompletionPorts.ToString() },
                    new [] { nameof(pool.MaxCompletionPorts), pool.MaxCompletionPorts.ToString() },
                    new [] {nameof(pool.MaxFreeCompletionPorts), pool.MaxFreeCompletionPorts.ToString() },
                });
        }
    }
}