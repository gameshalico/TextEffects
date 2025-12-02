using TextEffects.Data;

namespace TextEffects.Core
{
    internal interface ITextAnimationHandler
    {
        void UpdateText(AnimationTextInfo animationInfo);
        void Setup(TextInfo textInfo);
        void Release();
    }
}