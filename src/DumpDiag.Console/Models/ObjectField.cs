namespace DumpDiag.Console.Models
{
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
}