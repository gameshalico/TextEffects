using System.Collections.Generic;
using TextEffects.Core;
using TextEffects.Data;
using TMPro;

namespace TextEffects.Effects.Typewriter
{
    public interface IScriptListener
    {
        public void OnSetupCompleted(TextInfo textInfo, IReadOnlyCollection<TagInfo> tags)
        {
        }

        public void OnReleased()
        {
        }

        public void OnScriptModified(ScriptTextInfo scriptInfo)
        {
        }

        public void OnPlayStarted()
        {
        }

        public void OnPlayFinished()
        {
        }

        public void OnPlayCanceled()
        {
        }

        public void OnPaused()
        {
        }

        public void OnResumed()
        {
        }

        public void OnCharacterShown(int characterIndex)
        {
        }

        public void OnCharacterHidden(int characterIndex)
        {
        }
    }
}