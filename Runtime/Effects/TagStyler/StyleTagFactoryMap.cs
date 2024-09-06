using System.Collections.Generic;
using TextEffects.Data;
using TextEffects.Effects.TagStyler.StyleTags;

namespace TextEffects.Effects.TagStyler
{
    public class StyleTagFactoryMap : IStyleTagFactory
    {
        private readonly Dictionary<string, IStyleTagFactory> _factories;

        private StyleTagFactoryMap(Dictionary<string, IStyleTagFactory> factories)
        {
            _factories = factories;
        }

        public static StyleTagFactoryMap Default { get; } = new(new Dictionary<string, IStyleTagFactory>
        {
            { "wave", new PooledStyleTag<WaveStyleTag>.Factory() },
            { "incr", new PooledStyleTag<IncreasingSizeStyleTag>.Factory() },
            { "rainb", new PooledStyleTag<RainbowStyleTag>.Factory() },
            { "swing", new PooledStyleTag<SwingStyleTag>.Factory() },
            { "pend", new PooledStyleTag<PendulumStyleTag>.Factory() },
            { "shake", new PooledStyleTag<ShakeStyleTag>.Factory() },
            { "wiggle", new PooledStyleTag<WiggleStyleTag>.Factory() },
            { "bounce", new PooledStyleTag<BounceStyleTag>.Factory() },
            { "warp", new PooledStyleTag<WarpStyleTag>.Factory() }
        });

        public IStyleTag CreateTag(TagInfo tagInfo)
        {
            if (_factories.TryGetValue(tagInfo.TagName, out var factory))
                return factory.CreateTag(tagInfo);
            return null;
        }

        public void RegisterFactory(string tagName, IStyleTagFactory factory)
        {
            _factories.Add(tagName, factory);
        }

        public void UnregisterFactory(string tagName)
        {
            _factories.Remove(tagName);
        }
    }
}