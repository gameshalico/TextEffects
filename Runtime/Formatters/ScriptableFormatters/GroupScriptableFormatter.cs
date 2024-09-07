using System;
using System.Linq;
using UnityEngine;

namespace TextEffects.Formatters.ScriptableFormatters
{
    [CreateAssetMenu(fileName = "Group Scriptable Formatter",
        menuName = "Text Effects/Formatters/Group Scriptable Formatter")]
    public class GroupScriptableFormatter : ScriptableFormatter
    {
        [SerializeField] private int _order;
        [SerializeField] private ScriptableFormatter[] _formatters;
        private Action _onTextChanged;

        private ScriptableFormatter[] _subscribedFormatters;

        public override int FormatOrder => _order;

        private void OnValidate()
        {
            _onTextChanged?.Invoke();

            if (_subscribedFormatters != null)
                foreach (var formatter in _subscribedFormatters)
                {
                    if (formatter == null)
                        continue;
                    formatter.OnTextChanged -= Refresh;
                }

            _subscribedFormatters = new ScriptableFormatter[_formatters.Length];
            for (var i = 0; i < _formatters.Length; i++)
            {
                _subscribedFormatters[i] = _formatters[i];
                if (_subscribedFormatters[i] == null)
                    continue;
                _subscribedFormatters[i].OnTextChanged += Refresh;
            }
        }

        [ContextMenu("Refresh")]
        private void Refresh()
        {
            _onTextChanged?.Invoke();
        }

        public override event Action OnTextChanged
        {
            add => _onTextChanged += value;
            remove => _onTextChanged -= value;
        }

        public override string FormatText(string input)
        {
            foreach (var replacementDictionary in _formatters.OrderBy(formatter => formatter.FormatOrder))
                input = replacementDictionary.FormatText(input);

            return input;
        }
    }
}