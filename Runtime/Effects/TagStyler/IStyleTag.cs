using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.TagStyler
{
    public interface IStyleTag
    {
        void UpdateText(AnimationTextInfo animationInfo);
        void Release();
    }
}