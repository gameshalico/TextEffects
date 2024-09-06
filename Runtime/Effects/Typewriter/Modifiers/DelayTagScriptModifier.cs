using System.Collections.Generic;
using System.Linq;
using TextEffects.Data;

namespace TextEffects.Effects.Typewriter.Modifiers
{
    public class DelayTagScriptModifier : IScriptModifier
    {
        public void ModifyScript(IReadOnlyCollection<TagInfo> tags, ScriptInfo scriptInfo)
        {
            // delay rate
            foreach (var tag in tags.Where(tag => tag.TagName == "dr" || tag.TagName == "delay-rate"))
            {
                var delayRate = tag.GetFloat("", 1.0f);
                if (tag.StartIndex >= scriptInfo.ScriptCharacterInfo.Length) continue;

                if (tag.IsEmptyTag)
                {
                    scriptInfo.ScriptCharacterInfo[tag.StartIndex].Delay *= delayRate;
                    continue;
                }

                for (var i = tag.StartIndex; i < tag.EndIndex; i++)
                    scriptInfo.ScriptCharacterInfo[i].Delay *= delayRate;
            }

            // delay
            foreach (var tag in tags.Where(tag => tag.TagName == "d" || tag.TagName == "delay"))
            {
                var delay = tag.GetFloat("", 0.2f);

                if (tag.IsEmptyTag)
                {
                    if (tag.StartIndex >= scriptInfo.ScriptCharacterInfo.Length) scriptInfo.LastDelay += delay;
                    else
                        scriptInfo.ScriptCharacterInfo[tag.StartIndex].Delay += delay;
                    continue;
                }

                for (var i = tag.StartIndex; i < tag.EndIndex; i++) scriptInfo.ScriptCharacterInfo[i].Delay += delay;
            }
        }
    }
}