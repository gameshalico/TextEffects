using TMPro;

namespace TextEffects.Data
{
    public struct AnimationTextInfo
    {
        public AnimationCharacterInfo[] AnimationCharacterInfo;

        public AnimationTextInfo(AnimationCharacterInfo[] animationCharacterInfo)
        {
            AnimationCharacterInfo = animationCharacterInfo;
        }
    }
}