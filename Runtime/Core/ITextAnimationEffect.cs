using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    public interface ITextAnimationEffect
    {
        public void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags);
        public void UpdateCharacter(ref TMP_CharacterInfo characterInfo, ref CharacterAnimationInfo animationInfo);
        public void Release();
    }
}