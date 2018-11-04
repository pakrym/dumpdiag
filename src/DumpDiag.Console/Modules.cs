using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    internal class Modules : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession)
        {
            var modules = analysisSession.Context.Runtime.Modules;
            var sharedRuntimePaths = new[] { "Microsoft.NetCore.App", "Microsoft.AspNetCore.App" };
            Dictionary<string, string> frameworks = new Dictionary<string, string>();
            HashSet<ClrModule> runtimeModules = new HashSet<ClrModule>();
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
                            runtimeModules.Add(clrModule);
                        }
                    }
                }
            }

            analysisSession.Reporter.Table("Detected frameworks", frameworks.ToTable("Name", "Version"));


            analysisSession.Reporter.Table("Non-shared runtime Modules",
                analysisSession.Context.Runtime.Modules.
                Where(m=>!runtimeModules.Contains(m)).
                ToTable(
                "Name", "Informational version",
                module => module.Name, module => module.CreateMetadataReader().GetAssemblyAttributeStringValue("AssemblyInformationalVersionAttribute")));

        }
    }
}