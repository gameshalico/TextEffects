using TMPro;

namespace TextEffects.Data
{
    public struct TextAnimationInfo
    {
        public TMP_TextInfo TextInfo;
        public CharacterAnimationInfo[] CharacterAnimationInfo;

        public TextAnimationInfo(TMP_TextInfo textInfo, CharacterAnimationInfo[] characterAnimationInfo)
        {
            TextInfo = textInfo;
            CharacterAnimationInfo = characterAnimationInfo;
        }
    }
}