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
            else
            {
                return new ObjectInstance(clrObject);
            }
        }
    }
}