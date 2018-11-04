using System;
using System.Collections.Generic;
using System.IO;

namespace DumpDiag.Console
{
    internal class SharedRuntimes : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            var modules = analysisSession.Context.Runtime.Modules;
            var sharedRuntimePaths = new[] { "Microsoft.NetCore.App", "Microsoft.AspNetCore.App" };
            Dictionary<string, string> frameworks = new Dictionary<string, string>();
            foreach (var sharedRuntime in sharedRuntimePaths)
            {
                var sharedRuntimePath = Path.Combine("shared", sharedRuntime) + Path.DirectorySeparatorChar;
                foreach (var clrModule in modules)
                {

                    var fileName = clrModule.FileName;
                    if (fileName == null)
                    {
                        continue;

                    }
                    var index = fileName.IndexOf(sharedRuntimePath, StringComparison.OrdinalIgnoreCase);
                    if (index != -1)
                    {
                        var nextSlashIndex = fileName.IndexOf(Path.DirectorySeparatorChar, index + sharedRuntimePath.Length);
                        if (nextSlashIndex != -1)
                        {
                            var version = fileName.Substring(index + sharedRuntimePath.Length, nextSlashIndex - (index + sharedRuntimePath.Length));
                            frameworks[sharedRuntime] = version;
                        }
                    }
                }
            }

            analysisSession.Reporter.Table("Detected frameworks", frameworks.ToTable("Name", "Version"));
        }
    }
}