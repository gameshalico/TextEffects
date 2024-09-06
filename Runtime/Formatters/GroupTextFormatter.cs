using System;
using UnityEngine;

namespace TextEffects.Formatters
{
    [CreateAssetMenu(fileName = "TextFormatter Group", menuName = "TypeWriter/Text Formatter/Group Text Formatter")]
    public class GroupTextFormatter : TextFormatterBase
    {
        [SerializeField] private TextFormatterBase[] _replacementDictionaries;

        private Action _onReplacementsChanged;

        private TextFormatterBase[] _subscribedDictionaries;

        private void OnValidate()
        {
            _onReplacementsChanged?.Invoke();

            if (_subscribedDictionaries != null)
                foreach (var replacementDictionary in _subscribedDictionaries)
                    replacementDictionary.OnTextChanged -= Refresh;

            _subscribedDictionaries = new TextFormatterBase[_replacementDictionaries.Length];
            for (var i = 0; i < _replacementDictionaries.Length; i++)
            {
                _subscribedDictionaries[i] = _replacementDictionaries[i];
                _subscribedDictionaries[i].OnTextChanged += Refresh;
            }
        }

        [ContextMenu("Refresh")]
        private void Refresh()
        {
            _onReplacementsChanged?.Invoke();
        }

        public override event Action OnTextChanged
        {
            add => _onReplacementsChanged += value;
            remove => _onReplacementsChanged -= value;
        }

        public override string FormatText(string input)
        {
            foreach (var replacementDictionary in _replacementDictionaries)
                input = replacementDictionary.FormatText(input);

            return input;
        }
    }
}