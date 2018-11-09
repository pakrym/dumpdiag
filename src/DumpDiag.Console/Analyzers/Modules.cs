using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    internal class ModulesAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession, string[] arguments)
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

            analysisSession.Reporter.Write(new Table("Detected frameworks", frameworks.ToTable("Name", "Version")));


            analysisSession.Reporter.Write(new Table("Non-shared runtime Modules",
                analysisSession.Context.Runtime.Modules.Where(m => !runtimeModules.Contains(m)).ToTable(
                    module =>
                    {

                        string version = string.Empty;
                        string infoVersion = "";

                        var reader = module.CreateMetadataReader();
                        if (reader != null)
                        {
                            version = reader.GetAssemblyDefinition().Version.ToString();

                            var infVersionValue = reader.GetAssemblyAttributeStringValue("AssemblyInformationalVersionAttribute");
                            if (!infVersionValue.FixedArguments.IsDefaultOrEmpty)
                            {
                                infoVersion = infVersionValue.FixedArguments[0].Value.ToString();
                            }
                        }
                        return new[]
                        {
                            module.Name ?? "<empty>",
                            version,
                            infoVersion
                        };
                    }, "Name", "Version", "Informational version")));
        }
    }
}