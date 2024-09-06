using System.Collections.Generic;
using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.Modifiers
{
    public class DefaultScriptModifier : IScriptModifier
    {
        public DefaultScriptModifier(float defaultDelay)
        {
            DefaultDelay = defaultDelay;
        }

        public float DefaultDelay { get; set; }

        public void ModifyScript(IReadOnlyCollection<TagInfo> tags, ScriptTextInfo scriptInfo)
        {
            for (var i = 0; i < scriptInfo.ScriptCharacterInfo.Length; i++)
                scriptInfo.ScriptCharacterInfo[i].Delay = DefaultDelay;
        }
    }
}