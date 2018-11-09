namespace DumpDiag.Console
{
    public class FormattedString
    {
        public object[] Objects { get; }

        public FormattedString(params object[] objects)
        {
            Objects = objects;
        }
    }
}