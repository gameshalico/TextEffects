using System;
using System.Collections.Generic;
using System.Linq;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter.Listeners
{
    public readonly struct TagEventData
    {
        public int CharacterIndex { get; init; }
        public Dictionary<string, string> Attributes { get; init; }
        public int StartIndex { get; init; }
        public int EndIndex { get; init; }
        public bool IsEmptyTag { get; init; }
        public int Length => EndIndex - StartIndex;
    }

    public class TagEventDispatcherScriptListener : IScriptListener
    {
        private TagInfo[] _triggerTags;

        private Action<TagEventData> _onEventTriggered;

        void IScriptListener.OnSetup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
            _triggerTags = tags
                .Where(tag => tag.TagName == "evt" || tag.TagName == "event")
                .OrderBy(tag => tag.StartIndex)
                .ToArray();
        }

        void IScriptListener.OnCharacterShow(int characterIndex)
        {
            if (_triggerTags == null || _triggerTags.Length == 0)
                return;

            foreach (var tag in _triggerTags)
            {
                if (tag.StartIndex > characterIndex)
                    break;

                if ((tag.IsEmptyTag && tag.StartIndex == characterIndex) ||
                    (!tag.IsEmptyTag && tag.IsInRange(characterIndex)))
                    TriggerEvent(tag, characterIndex);
            }
        }

        public event Action<TagEventData> OnEventTriggered
        {
            add => _onEventTriggered += value;
            remove => _onEventTriggered -= value;
        }

        private void TriggerEvent(TagInfo tag, int index)
        {
            _onEventTriggered?.Invoke(new TagEventData
            {
                CharacterIndex = index,
                Attributes = tag.CloneAttributes(),
                StartIndex = tag.StartIndex,
                EndIndex = tag.EndIndex,
                IsEmptyTag = tag.IsEmptyTag
            });
        }
    }
}