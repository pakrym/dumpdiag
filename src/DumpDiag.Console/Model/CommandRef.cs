namespace DumpDiag.Console
{
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