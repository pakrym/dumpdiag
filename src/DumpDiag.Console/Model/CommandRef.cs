using System.Collections.Generic;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    public class ObjectInstance
    {
        public ObjectInstance(ClrObject clrObject)
        {
            TypeName = clrObject.Type.Name;
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

        public ObjectField[] Fields { get; }
        public string TypeName { get; }
    }

    public class StringInstance
    {
        public string Value { get; }

        public StringInstance(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"\"{Value}\"";
        }
    }

    public class NullValue
    {
        public static  NullValue Instance { get; } = new NullValue();
    }

    public class ObjectRef
    {
        public ulong Address { get; }

        public ObjectRef(ulong address)
        {
            Address = address;
        }

        public override string ToString()
        {
            return $"{Address:X}";
        }
    }

    public class ObjectField
    {
        public ObjectField(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public  object Value { get; set; }
    }

    public class CommandRef
    {
        public string Name { get; }

        public CommandRef(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}