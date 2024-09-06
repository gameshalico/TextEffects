using System;
using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Formatters
{
    public abstract class TextFormatterBase : ScriptableObject, ITextFormatter
    {
        public abstract string FormatText(string text);
        public abstract event Action OnTextChanged;
    }
}