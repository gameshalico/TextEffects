namespace TextEffects.Core
{
    public interface ITextFormatter
    {
        public int FormatOrder { get; }
        public string FormatText(string text);
    }
}