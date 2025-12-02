using TextEffects.Data;

namespace TextEffects.Core
{
    internal interface ITextAnimationHandler
    {
        void Setup(TextInfo textInfo);
        void UpdateText(AnimationTextInfo animationInfo);
        void Release();
    }
}