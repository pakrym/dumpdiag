using System.Collections.Generic;

namespace DumpDiag.Console.Models
{
    public class HeapStatistics
    {
        public int TotalUniqueObjects { get; set;  }
        public IList<HeapTypeStatistics> Types { get; set;  }
    }
}
