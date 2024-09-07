using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextEffects.Formatters.ScriptableFormatters
{
    [CreateAssetMenu(fileName = "Replace Formatter", menuName = "Text Effects/Formatters/Replace Formatter")]
    public class ReplaceScriptableFormatter : ScriptableFormatter
    {
        [SerializeField] private int _order;
        [SerializeField] private ReplacementData[] _replacements;

        private Regex _cachedReplacementRegex;
        private IReadOnlyDictionary<string, string> _cachedReplacements;
        private bool _needToRefresh = true;

        private Action _onTextChanged;
        public override int FormatOrder => _order;

        private void OnValidate()
        {
            _needToRefresh = true;
            _onTextChanged?.Invoke();
        }

        public override event Action OnTextChanged
        {
            add => _onTextChanged += value;
            remove => _onTextChanged -= value;
        }

        public override string FormatText(string input)
        {
            CacheReplacementsIfNeeded();
            return _cachedReplacementRegex.Replace(input, match => _cachedReplacements[match.Value]);
        }


        private void CacheReplacementsIfNeeded()
        {
            if (!_needToRefresh && _cachedReplacements != null)
                return;

            var replacements = new Dictionary<string, string>(_replacements.Length);
            foreach (var replacement in _replacements) replacements[replacement.Target] = replacement.Replacement;

            _cachedReplacements = replacements;
            _cachedReplacementRegex = new Regex(string.Join("|", replacements.Keys));
            _needToRefresh = false;
        }

        [Serializable]
        public struct ReplacementData
        {
            public string Target;
            public string Replacement;
        }
    }
}