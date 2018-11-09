using DumpDiag.Console.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DumpDiag.Console.Controllers
{
    [Route("commands")]
    public class CommandsController : Controller
    {
        [Route("heapstats")]
        public ActionResult HeapStats()
        {
            var data = new HeapStatistics
            {
                TotalUniqueObjects = 4,
                Types = new List<HeapTypeStatistics>()
                {
                    new HeapTypeStatistics() { Count = 1, TotalSize = 10, TypeName = "System.IO.Pipelines.Pipe" },
                    new HeapTypeStatistics() { Count = 4, TotalSize = 100, TypeName = "Microsoft.AspNetCore.Http.HttpContext" },
                    new HeapTypeStatistics() { Count = 1, TotalSize = 1000, TypeName = "Microsoft.AspNetCore.SignalR.HubConnectionContext" },
                    new HeapTypeStatistics() { Count = 4000, TotalSize = long.MaxValue, TypeName = "System.Byte[]" },
                }
            };
            return View(data);
        }

        [Route("types/{typeName}/instances")]
        public ActionResult ListInstances(string typeName)
        {
            return Content($"Instances of {typeName}");
        }
    }
}
