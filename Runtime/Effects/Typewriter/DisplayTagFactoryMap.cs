using System.Collections.Generic;
using TextEffects.Data;
using TextEffects.Effects.Typewriter.DisplayTags;

namespace TextEffects.Effects.Typewriter
{
    public class DisplayTagFactoryMap : IDisplayTagFactory
    {
        private readonly Dictionary<string, IDisplayTagFactory> _factories;

        private DisplayTagFactoryMap(Dictionary<string, IDisplayTagFactory> factories)
        {
            _factories = factories;
        }

        public static DisplayTagFactoryMap Default { get; } = new(new Dictionary<string, IDisplayTagFactory>
        {
            { "@fade", new PooledDisplayTag<FadeShowTag>.Factory() },
            { "~fade", new PooledDisplayTag<FadeHideTag>.Factory() },

            { "@scale", new PooledDisplayTag<ScaleShowTag>.Factory() },
            { "~scale", new PooledDisplayTag<ScaleHideTag>.Factory() },

            { "@offset", new PooledDisplayTag<OffsetShowTag>.Factory() },
            { "~offset", new PooledDisplayTag<OffsetHideTag>.Factory() },

            { "@rdir", new PooledDisplayTag<RandomOffsetShowTag>.Factory() },
            { "~rdir", new PooledDisplayTag<RandomOffsetHideTag>.Factory() },

            { "@rotate", new PooledDisplayTag<RotateShowTag>.Factory() },
            { "~rotate", new PooledDisplayTag<RotateHideTag>.Factory() }
        });

        public IDisplayTag CreateTag(TagInfo tagInfo)
        {
            if (_factories.TryGetValue(tagInfo.TagName, out var factory))
                return factory.CreateTag(tagInfo);
            return null;
        }

        public void RegisterFactory(string tagName, IDisplayTagFactory factory)
        {
            _factories.Add(tagName, factory);
        }
    }
}