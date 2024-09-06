using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Core
{
    public interface ITextAnimationEffect
    {
        public void Setup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags);
        void UpdateText(AnimationTextInfo animationInfo);
        public void Release();
    }
}