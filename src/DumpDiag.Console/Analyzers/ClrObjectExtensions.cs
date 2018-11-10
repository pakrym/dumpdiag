using DumpDiag.Console.Models;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    internal static class ClrObjectExtensions
    {
        public static object Describe(this ClrObject clrObject)
        {
            if (clrObject.Type.IsString)
            {
                return new StringInstance((string)clrObject.Type.GetValue(clrObject.Address));
            }

            if (clrObject.IsArray)
            {
                return new ArrayInstance(clrObject);
            }
            return new ObjectInstance(clrObject);
        }
    }
}