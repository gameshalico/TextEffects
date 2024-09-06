namespace TextEffects.Data
{
    public class ScriptInfo
    {
        public ScriptInfo(int charCount)
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

        public ScriptCharacterInfo[] ScriptCharacterInfo { get; }
    }
}