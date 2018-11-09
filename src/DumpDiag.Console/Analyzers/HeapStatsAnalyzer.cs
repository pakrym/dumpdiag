using DumpDiag.Console.Models;
using System;
using System.Linq;

namespace DumpDiag.Console.Analyzers
{
    public class HeapStatsAnalyzer : IDumpAnalyzer
    {
        public void Run(AnalysisSession analysisSession, string[] arguments)
        {
            var groups = analysisSession.Context.Runtime.Heap.EnumerateObjects().GroupBy(o => o.Type.Name).ToList();
            var heapStats = new HeapStatistics()
            {
                TotalUniqueObjects = groups.Count,
                Types = groups.Select(g =>
                {
                    var objs = g.ToList();
                    return new HeapTypeStatistics()
                    {
                        TypeName = Truncate(g.Key),
                        Count = objs.Count,
                        TotalSize = objs.Aggregate(0ul, (l, o) => l + o.Size)
                    };
                }).OrderByDescending(t => t.TotalSize)
            };
            analysisSession.Reporter.Write(heapStats);
        }

        private string Truncate(string key)
        {
            if(key.Length > 50)
            {
                return key.Substring(0, 47) + "...";
            }
            return key;
        }
    }
}
