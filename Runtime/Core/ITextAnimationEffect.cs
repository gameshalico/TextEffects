using System.Collections.Generic;
using TextEffects.Data;

namespace TextEffects.Core
{
    public interface ITextAnimationEffect
    {
        public void Setup(TextInfo textInfo, IReadOnlyCollection<TagInfo> tags);
        void UpdateText(AnimationTextInfo animationInfo);
        public void Release();
    }
}