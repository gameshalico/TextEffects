using System.Collections.Generic;
using TextEffects.Data;
using TextEffects.Effects.Typewriter.ScriptTags;

namespace TextEffects.Effects.Typewriter
{
    /// <summary>
    /// ScriptTagのファクトリーマップ
    /// タグ名とファクトリーを紐付けて管理します
    /// </summary>
    public sealed class ScriptTagFactoryMap : IScriptTagFactory
    {
        private readonly Dictionary<string, IScriptTagFactory> _factories;

        public ScriptTagFactoryMap(Dictionary<string, IScriptTagFactory> factories = null)
        {
            _factories = factories ?? new Dictionary<string, IScriptTagFactory>();
        }

        public ScriptTagFactoryMap Clone()
        {
            return new ScriptTagFactoryMap(new Dictionary<string, IScriptTagFactory>(_factories));
        }

        /// <summary>
        /// デフォルトのScriptTagFactoryMap
        /// </summary>
        public static ScriptTagFactoryMap Default { get; } = new();

        public IScriptTag CreateTag(TagInfo tagInfo)
        {
            if (_factories.TryGetValue(tagInfo.TagName, out var factory))
                return factory.CreateTag(tagInfo);
            return null;
        }

        /// <summary>
        /// カスタムScriptTagファクトリーを登録します
        /// </summary>
        /// <param name="tagName">タグ名</param>
        /// <param name="factory">ファクトリー</param>
        public void RegisterFactory(string tagName, IScriptTagFactory factory)
        {
            _factories[tagName] = factory;
        }

        /// <summary>
        /// 登録されたファクトリーを削除します
        /// </summary>
        /// <param name="tagName">タグ名</param>
        public void UnregisterFactory(string tagName)
        {
            _factories.Remove(tagName);
        }
    }
}
