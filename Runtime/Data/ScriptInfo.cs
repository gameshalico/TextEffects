namespace TextEffects.Data
{
    public class ScriptInfo
    {
        public ScriptInfo(int charCount)
        {
            CharacterInfos = new ScriptCharacterInfo[charCount];
            for (var i = 0; i < charCount; i++)
            {
                CharacterInfos[i] = new ScriptCharacterInfo
                {
                    CharacterIndex = i,
                    Delay = 0f
                };
                CharacterInfos[i].Reset();
            }
        }

        public float LastDelay { get; set; }

        public ScriptCharacterInfo[] CharacterInfos { get; }
    }
}