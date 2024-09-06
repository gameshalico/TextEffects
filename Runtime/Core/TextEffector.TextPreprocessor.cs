using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace TextEffects.Core
{
    public partial class TextEffector
    {
        public IReadOnlyCollection<ITextFormatter> Formatters => _textPreprocessor.Formatters;

        public void AddFormatter(ITextFormatter formatter)
        {
            InitializeIfNeeded();
            _textPreprocessor.AddFormatter(formatter);
            SetDirty();
        }

        public void RemoveFormatter(ITextFormatter formatter)
        {
            InitializeIfNeeded();
            _textPreprocessor.RemoveFormatter(formatter);
            SetDirty();
        }

        private class TextPreprocessor : ITextPreprocessor
        {
            private readonly List<ITextFormatter> _formatters = new();
            private readonly TextEffector _textEffector;

            public TextPreprocessor(TextEffector textEffector)
            {
                _textEffector = textEffector;
            }

            public IReadOnlyCollection<ITextFormatter> Formatters => _formatters;

            public string PreprocessText(string text)
            {
                var formattedText = text;
                foreach (var formatter in _formatters.OrderBy(f => f.FormatOrder))
                    formattedText = formatter.FormatText(formattedText);

                var parseResults = TagParser.Parse(formattedText);
                _textEffector._animationHandler.SetTags(parseResults.tags);

                return parseResults.tmpText;
            }

            public void AddFormatter(ITextFormatter formatter)
            {
                _formatters.Add(formatter);
            }

            public void RemoveFormatter(ITextFormatter formatter)
            {
                _formatters.Remove(formatter);
            }
        }
    }
}