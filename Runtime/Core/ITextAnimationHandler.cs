using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    internal interface ITextAnimationHandler
    {
        void UpdateText(TextAnimationInfo textAnimationInfo);
        void Setup(TMP_TextInfo textInfo);
        void Release();
    }
}