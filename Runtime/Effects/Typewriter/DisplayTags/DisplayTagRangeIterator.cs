using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.DisplayTags
{
    /// <summary>
    /// DisplayTagの範囲内の文字を反復処理するための構造体Enumerator
    /// </summary>
    public ref struct DisplayTagRangeEnumerator
    {
        private readonly AnimationTextInfo _animationInfo;
        private readonly ScriptTextInfo _scriptInfo;
        private readonly int _startIndex;
        private readonly int _endIndex;
        private int _currentIndex;

        public DisplayTagRangeEnumerator(
            TagInfo tagInfo,
            AnimationTextInfo animationInfo,
            ScriptTextInfo scriptInfo)
        {
            _animationInfo = animationInfo;
            _scriptInfo = scriptInfo;
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

        public DisplayTagRangeEnumerator Current => this;
        public ref AnimationCharacterInfo AnimationInfo => ref _animationInfo.AnimationCharacterInfo[_currentIndex];
        public ref ScriptCharacterInfo ScriptInfo => ref _scriptInfo.ScriptCharacterInfo[_currentIndex];
    }

    /// <summary>
    /// DisplayTagの範囲を反復処理可能にするための構造体
    /// </summary>
    public ref struct DisplayTagRange
    {
        private readonly TagInfo _tagInfo;
        private readonly AnimationTextInfo _animationInfo;
        private readonly ScriptTextInfo _scriptInfo;

        public DisplayTagRange(
            TagInfo tagInfo,
            AnimationTextInfo animationInfo,
            ScriptTextInfo scriptInfo)
        {
            _tagInfo = tagInfo;
            _animationInfo = animationInfo;
            _scriptInfo = scriptInfo;
        }

        public DisplayTagRangeEnumerator GetEnumerator()
        {
            return new DisplayTagRangeEnumerator(_tagInfo, _animationInfo, _scriptInfo);
        }
    }

    /// <summary>
    /// DisplayTagの範囲内の文字を反復処理するためのユーティリティクラス
    /// </summary>
    public static class DisplayTagRangeIterator
    {
        /// <summary>
        /// DisplayTagの範囲を取得します
        /// </summary>
        public static DisplayTagRange GetRange(
            TagInfo tagInfo,
            AnimationTextInfo animationInfo,
            ScriptTextInfo scriptInfo)
        {
            return new DisplayTagRange(tagInfo, animationInfo, scriptInfo);
        }
    }
}
