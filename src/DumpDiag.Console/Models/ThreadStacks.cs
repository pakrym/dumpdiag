using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console.Models
{
    public class ThreadStacks
    {
        public ThreadStacks(ClrThread runtimeThread)
        {
            ThreadId = runtimeThread.ManagedThreadId.ToString();
            StackFrames = runtimeThread.StackTrace.Select(st => new ThreadStackFrame(st)).ToArray();
        }

        public string ThreadId { get; }
        public ThreadStackFrame[] StackFrames { get; }
    }
}