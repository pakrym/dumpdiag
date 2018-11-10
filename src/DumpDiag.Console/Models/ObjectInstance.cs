using System.Collections.Generic;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console.Models
{
    public class ObjectInstance
    {
        public ObjectInstance(ClrObject clrObject)
        {
            Address= new ObjectRef(clrObject.Address);
            Type = new TypeRef(clrObject.Type.Name);
            var fields = new List<ObjectField>();
            foreach (var clrInstanceField in clrObject.Type.Fields)
            {
                object value = null;

                if (clrInstanceField.ElementType != ClrElementType.Object)
                {
                    value = clrInstanceField.GetValue(clrObject.Address);
                }
                else
                {
                    var address = (ulong)clrInstanceField.GetValue(clrObject.Address);
                    if (address != 0)
                    {
                        value = new ObjectRef(address);
                    }
                }

                fields.Add(new ObjectField(clrInstanceField.Name, value ?? NullValue.Instance));
            }

            Fields = fields.ToArray();
        }
        public ObjectRef Address { get; }
        public ObjectField[] Fields { get; }
        public TypeRef Type { get; }
    }
}