using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    internal interface ITextAnimationHandler
    {
        void UpdateCharacter(ref TMP_CharacterInfo characterInfo, ref CharacterAnimationInfo animationInfo);
        void Setup(TMP_TextInfo textInfo);
        void Release();
    }
}