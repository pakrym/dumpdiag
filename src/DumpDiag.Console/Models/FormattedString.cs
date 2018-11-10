namespace DumpDiag.Console.Models
{
    public class FormattedString
    {
        public object[] Objects { get; }

        public FormattedString(params object[] objects)
        {
            Objects = objects;
        }

        public override string ToString()
        {
            return string.Join("", Objects);
        }
    }
}