using TextEffects.Data;

namespace TextEffects.Effects.Typewriter
{
    public interface IDisplayTagFactory
    {
        IDisplayTag CreateTag(TagInfo tagInfo);
    }
}