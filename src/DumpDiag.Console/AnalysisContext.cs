using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    public class AnalysisContext
    {
        public static readonly AnalysisContext Empty = new AnalysisContext(null, null);

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
            var runtimeInfo = dataTarget.ClrVersions[0];  // just using the first runtime

            var runtime = string.IsNullOrEmpty(dacPath) ?
                runtimeInfo.CreateRuntime() :
                runtimeInfo.CreateRuntime(dacPath, true);
            return new AnalysisContext(dataTarget, runtime);
        }
    }
}
