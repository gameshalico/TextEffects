namespace TextEffects.Data
{
    public class ScriptTextInfo
    {
        public ScriptTextInfo(int charCount)
        {
            ScriptCharacterInfo = new ScriptCharacterInfo[charCount];
            for (var i = 0; i < charCount; i++)
            {
                ScriptCharacterInfo[i] = new ScriptCharacterInfo
                {
                    CharacterIndex = i,
                    Delay = 0f
                };
                ScriptCharacterInfo[i].Reset();
            }
        }

        public float LastDelay { get; set; }

        public ScriptCharacterInfo[] ScriptCharacterInfo { get; private set; }

        public void Resize(int charCount)
        {
            var newScriptCharacterInfo = new ScriptCharacterInfo[charCount];
            for (var i = 0; i < charCount; i++)
            {
                if (i < ScriptCharacterInfo.Length)
                {
                    newScriptCharacterInfo[i] = ScriptCharacterInfo[i];
                }
                else
                {
                    newScriptCharacterInfo[i] = new ScriptCharacterInfo
                    {
                        CharacterIndex = i
                    };
                    newScriptCharacterInfo[i].Reset();
                }

                newScriptCharacterInfo[i].Delay = 0f;
            }

            ScriptCharacterInfo = newScriptCharacterInfo;
        }
    }
}