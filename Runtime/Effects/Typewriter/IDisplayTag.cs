using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter
{
    public interface IDisplayTag
    {
        void UpdateText(AnimationTextInfo animationInfo, ScriptTextInfo scriptInfo);

        void Release();
    }
}