#if TEXTEFFECTS_UIELEMENTS_SUPPORT
using System;
using System.Collections.Generic;
using System.Linq;
using TextEffects.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace TextEffects.Core.UIElements
{
    public sealed class UIElementsTextEffector : IDisposable
    {
        private readonly TextElement _textElement;
        private readonly UIElementsAnimationApplier _applier;
        private readonly UIElementsTextAnimationHandler _handler;
        private readonly List<ITextFormatter> _formatters = new();

        private string _rawText;
        private bool _isDisposed;
        private bool _isProcessing;
        private IVisualElementScheduledItem _updateSchedule;

        public UIElementsTextEffector(TextElement textElement)
        {
            _textElement = textElement;
            _handler = new UIElementsTextAnimationHandler();
            _applier = new UIElementsAnimationApplier(_handler);

            _textElement.PostProcessTextVertices += OnPostProcessTextVertices;
            _updateSchedule = _textElement.schedule.Execute(Update).Every(16);
        }

        public IReadOnlyCollection<ITextAnimationEffect> Effects => _handler.Effects;

        public void SetText(string text)
        {
            _rawText = text;
            RefreshText();
        }

        public void AddEffect(ITextAnimationEffect effect)
        {
            _handler.AddEffect(effect);
            RefreshText();
        }

        public void RemoveEffect(ITextAnimationEffect effect)
        {
            _handler.RemoveEffect(effect);
            RefreshText();
        }

        public void AddFormatter(ITextFormatter formatter)
        {
            _formatters.Add(formatter);
            RefreshText();
        }

        public void RemoveFormatter(ITextFormatter formatter)
        {
            _formatters.Remove(formatter);
            RefreshText();
        }

        private void RefreshText()
        {
            if (string.IsNullOrEmpty(_rawText))
            {
                _textElement.text = string.Empty;
                return;
            }

            var formattedText = _rawText;
            foreach (var formatter in _formatters.OrderBy(f => f.FormatOrder))
            {
                formattedText = formatter.FormatText(formattedText);
            }

            var (text, tags) = TagParser.Parse(formattedText, unescapeXml: true);
            _handler.SetTags(tags);

            var characterCount = text.Length;
            _applier.Refresh(characterCount);

            _textElement.text = text;
        }

        private void OnPostProcessTextVertices(TextElement.GlyphsEnumerable glyphs)
        {
            if (_isDisposed) return;
            _isProcessing = true;
            try
            {
                _applier.ProcessGlyphs(glyphs);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void Update()
        {
            if (_isDisposed || _isProcessing) return;
            _textElement.MarkDirtyRepaint();
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            _updateSchedule?.Pause();
            _textElement.PostProcessTextVertices -= OnPostProcessTextVertices;
            _handler.Release();
        }
    }
}
#endif
