using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console.Models
{
    public class MethodRef
    {
        public MethodRef(ClrMethod method)
        {
            Type = new TypeRef(method.Type.Name);
            Name = method.Name;
        }

        public TypeRef Type { get; }
        public string Name { get; }
    }
}