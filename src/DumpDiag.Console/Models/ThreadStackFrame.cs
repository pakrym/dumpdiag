using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console.Models
{
    public class ThreadStackFrame
    {
        public ThreadStackFrame(ClrStackFrame clrStackFrame)
        {
            if (clrStackFrame.Method != null)
            {

            Method = new MethodRef(clrStackFrame.Method);
            }
            DisplayName = clrStackFrame.DisplayString;
        }

        public MethodRef Method { get; }
        public string DisplayName { get; }
    }
}