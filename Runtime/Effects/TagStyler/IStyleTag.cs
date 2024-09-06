using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler
{
    public interface IStyleTag
    {
        void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags);
        void UpdateText(TextAnimationInfo textAnimationInfo);
        void Release();
    }
}