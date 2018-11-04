using System;
using System.Collections.Generic;

namespace DumpDiag.Console
{
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
}