using TMPro;

namespace TextEffects.Data
{
    public struct AnimationTextInfo
    {
        public TMP_TextInfo TextInfo;
        public AnimationCharacterInfo[] AnimationCharacterInfo;

        public AnimationTextInfo(TMP_TextInfo textInfo, AnimationCharacterInfo[] animationCharacterInfo)
        {
            TextInfo = textInfo;
            AnimationCharacterInfo = animationCharacterInfo;
        }
    }
}