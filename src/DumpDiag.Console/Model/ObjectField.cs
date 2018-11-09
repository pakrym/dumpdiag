namespace DumpDiag.Console
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