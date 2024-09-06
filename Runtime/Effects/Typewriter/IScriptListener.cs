using System.Collections.Generic;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter
{
    public interface IScriptListener
    {
        public void OnSetup(TMP_TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
        }

        public void OnRelease()
        {
        }

        public void OnScriptModify(ScriptInfo scriptInfo)
        {
        }

        public void OnPlay()
        {
        }

        public void OnFinish()
        {
        }

        public void OnCharacterShow(int characterIndex)
        {
        }

        public void OnCharacterHide(int characterIndex)
        {
        }
    }
}