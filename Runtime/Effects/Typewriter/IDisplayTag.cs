using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter
{
    public interface IDisplayTag
    {
        void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags);
        void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo);

        void Release();
    }
}