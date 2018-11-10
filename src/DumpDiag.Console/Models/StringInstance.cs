namespace DumpDiag.Console.Models
{
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
}