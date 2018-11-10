namespace DumpDiag.Console.Models
{
    public class HeapTypeStatistics
    {
        public int Count { get; set; }
        public ulong TotalSize { get; set; }
        public TypeRef TypeName { get; set; }
    }
}
