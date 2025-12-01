using TextEffects.Data;

namespace TextEffects.Effects.Typewriter
{
    /// <summary>
    /// ScriptTagを生成するファクトリーインターフェース
    /// </summary>
    public interface IScriptTagFactory
    {
        /// <summary>
        /// TagInfoからScriptTagを生成する
        /// </summary>
        /// <param name="tagInfo">タグ情報</param>
        /// <returns>生成されたScriptTag。該当しない場合はnull</returns>
        IScriptTag CreateTag(TagInfo tagInfo);
    }
}
