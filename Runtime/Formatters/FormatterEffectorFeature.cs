using TextEffects.Core;

namespace TextEffects.Formatters
{
    public abstract class FormatterEffectorFeature : TextEffectorFeature, ITextFormatter
    {
        public abstract int FormatOrder { get; }
        public abstract string FormatText(string text);

        protected override void AddFeature(TextEffector textEffector)
        {
            textEffector.AddFormatter(this);
        }

        protected override void RemoveFeature(TextEffector textEffector)
        {
            textEffector.RemoveFormatter(this);
        }
    }
}