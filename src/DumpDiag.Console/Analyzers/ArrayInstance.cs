using System.Collections.Generic;
using DumpDiag.Console.Models;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console.Models
{
    public class ArrayInstance
    {
        public ArrayInstance(ClrObject clrObject)
        {
            var objectRefs = clrObject.Type.GetArrayLength(clrObject.Address);
            var objects = new List<ObjectRef>();

            for (int i = 0; i < objectRefs; i++)
            {
                objects.Add(new ObjectRef(clrObject.Type.GetArrayElementAddress(clrObject.Address, i)));
            }

            Address = new ObjectRef(clrObject.Address);
            Objects = objects.ToArray();
            ItemType = new TypeRef(clrObject.Type.Name.Substring(0, clrObject.Type.Name.Length - 2));
        }

        public ObjectRef Address { get; }
        public TypeRef ItemType { get; }
        public ObjectRef[] Objects { get; }
    }
}