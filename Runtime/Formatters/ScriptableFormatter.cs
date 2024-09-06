using System;
using TextEffects.Core;
using UnityEngine;

namespace TextEffects.Formatters
{
    public abstract class ScriptableFormatter : ScriptableObject, ITextFormatter
    {
        public abstract int FormatOrder { get; }
        public abstract string FormatText(string text);
        public abstract event Action OnTextChanged;
    }
}