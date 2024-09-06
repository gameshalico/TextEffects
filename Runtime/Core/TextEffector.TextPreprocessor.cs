using TMPro;

namespace TextEffects.Core
{
    public partial class TextEffector
    {
        public void AddFormatter(ITextFormatter formatter)
        {
            _formatters.Add(formatter);
            SetDirty();
        }

        public void RemoveFormatter(ITextFormatter formatter)
        {
            _formatters.Remove(formatter);
            SetDirty();
        }

        private class TextPreprocessor : ITextPreprocessor
        {
            private readonly TextEffector _textEffector;

            public TextPreprocessor(TextEffector textEffector)
            {
                _textEffector = textEffector;
            }

            public string PreprocessText(string text)
            {
                var formattedText = text;
                foreach (var formatter in _textEffector._formatters)
                    formattedText = formatter.FormatText(formattedText);

                var parseResults = TagParser.Parse(formattedText);
                _textEffector._animationHandler.SetTags(parseResults.tags);

                return parseResults.tmpText;
            }
        }
    }
}