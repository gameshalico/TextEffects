using System;
using System.Collections.Generic;
using System.Linq;
using TextEffects.Common;

namespace TextEffects.Data
{
    internal class TagInfoBuffer : PooledItem<TagInfoBuffer>, IEquatable<TagInfoBuffer>
    {
        public string TagName { get; private set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
        public int StartIndex { get; private set; }
        public int EndIndex { get; set; }
        public bool IsEmptyTag { get; set; }

        public bool Equals(TagInfoBuffer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TagName == other.TagName && Attributes.SequenceEqual(other.Attributes) &&
                   StartIndex == other.StartIndex &&
                   EndIndex == other.EndIndex && IsEmptyTag == other.IsEmptyTag;
        }

        protected override void OnReturn()
        {
            TagName = null;
            Attributes.Clear();
            StartIndex = 0;
            EndIndex = 0;
            IsEmptyTag = false;
        }

        public TagInfoBuffer Set(string tagName, int startIndex, int endIndex,
            bool isEmptyTag)
        {
            TagName = tagName;
            StartIndex = startIndex;
            EndIndex = endIndex;
            IsEmptyTag = isEmptyTag;

            return this;
        }
    }

    public readonly partial struct TagInfo : IEquatable<TagInfo>
    {
        public bool IsValid => _handle.IsValid;
        private readonly PooledHandle<TagInfoBuffer> _handle;

        internal TagInfo(TagInfoBuffer handle)
        {
            _handle = handle;
        }

        public string TagName => _handle.Item.TagName;
        public int StartIndex => _handle.Item.StartIndex;
        public int EndIndex => _handle.Item.EndIndex;
        public bool IsEmptyTag => _handle.Item.IsEmptyTag;
        private IReadOnlyDictionary<string, string> Attributes => _handle.Item.Attributes;

        public int Length
        {
            get
            {
                var item = _handle.Item;
                return item.EndIndex - item.StartIndex;
            }
        }

        public bool Equals(TagInfo other)
        {
            if (_handle.Equals(other._handle))
                return true;

            return _handle.Item.Equals(other._handle.Item);
        }

        public bool IsInRange(int index)
        {
            var item = _handle.Item;
            return item.StartIndex <= index && index < item.EndIndex;
        }

        public override string ToString()
        {
            var item = _handle.Item;
            var attributes = string.Join(" ", item.Attributes);
            if (item.IsEmptyTag)
                return $"<{item.TagName}:{item.StartIndex} {attributes}>";
            return $"<{item.TagName}:[{item.StartIndex},{item.EndIndex}) {attributes}>";
        }

        internal void Return()
        {
            _handle.Return();
        }

        public static TagInfo Create(string tagName, Dictionary<string, string> attributes, int startIndex,
            int endIndex, bool isEmptyTag)
        {
            var buffer = TagInfoBuffer.Rent();
            buffer.Attributes = attributes;
            return new TagInfo(buffer.Set(tagName, startIndex, endIndex, isEmptyTag));
        }

        public static bool operator ==(TagInfo left, TagInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TagInfo left, TagInfo right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is TagInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }
    }
}