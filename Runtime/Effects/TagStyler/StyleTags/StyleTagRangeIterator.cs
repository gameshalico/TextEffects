using TextEffects.Data;

namespace TextEffects.Effects.TagStyler.StyleTags
{
    /// <summary>
    /// StyleTagの範囲内の文字を反復処理するための構造体Enumerator
    /// </summary>
    public ref struct StyleTagRangeEnumerator
    {
        private readonly AnimationTextInfo _animationInfo;
        private readonly int _startIndex;
        private readonly int _endIndex;
        private int _currentIndex;

        public StyleTagRangeEnumerator(
            TagInfo tagInfo,
            AnimationTextInfo animationInfo)
        {
            _animationInfo = animationInfo;
            _startIndex = tagInfo.StartIndex;
            _endIndex = tagInfo.EndIndex;
            _currentIndex = _startIndex - 1;
        }

        public bool MoveNext()
        {
            while (++_currentIndex < _endIndex)
            {
                ref var characterAnimationInfo = ref _animationInfo.AnimationCharacterInfo[_currentIndex];
                if (characterAnimationInfo.IsInitialized)
                    return true;
            }
            return false;
        }

        public StyleTagRangeEnumerator Current => this;
        public ref AnimationCharacterInfo AnimationInfo => ref _animationInfo.AnimationCharacterInfo[_currentIndex];
    }

    /// <summary>
    /// StyleTagの範囲を反復処理可能にするための構造体
    /// </summary>
    public ref struct StyleTagRange
    {
        private readonly TagInfo _tagInfo;
        private readonly AnimationTextInfo _animationInfo;

        public StyleTagRange(
            TagInfo tagInfo,
            AnimationTextInfo animationInfo)
        {
            _tagInfo = tagInfo;
            _animationInfo = animationInfo;
        }

        public StyleTagRangeEnumerator GetEnumerator()
        {
            return new StyleTagRangeEnumerator(_tagInfo, _animationInfo);
        }
    }

    /// <summary>
    /// StyleTagの範囲内の文字を反復処理するためのユーティリティクラス
    /// </summary>
    public static class StyleTagRangeIterator
    {
        /// <summary>
        /// StyleTagの範囲を取得します
        /// </summary>
        public static StyleTagRange GetRange(
            TagInfo tagInfo,
            AnimationTextInfo animationInfo)
        {
            return new StyleTagRange(tagInfo, animationInfo);
        }
    }
}
