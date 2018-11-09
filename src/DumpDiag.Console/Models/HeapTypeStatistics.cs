namespace DumpDiag.Console.Models
{
    public class HeapTypeStatistics
    {
        public int Count { get; set; }
        public ulong TotalSize { get; set; }
        public string TypeName { get; set; }
    }
}
