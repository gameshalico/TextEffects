using System.Collections.Generic;
using TextEffects.Data;

namespace TextEffects.Effects.Typewriter
{
    public interface IScriptModifier
    {
        void ModifyScript(IReadOnlyCollection<TagInfo> tags, ScriptTextInfo scriptInfo);
    }
}