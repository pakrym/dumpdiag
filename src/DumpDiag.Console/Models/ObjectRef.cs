namespace DumpDiag.Console.Models
{
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
}