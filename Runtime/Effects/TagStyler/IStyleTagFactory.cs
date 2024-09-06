using TextEffects.Data;

namespace TextEffects.Effects.TagStyler
{
    public interface IStyleTagFactory
    {
        IStyleTag CreateTag(TagInfo tagInfo);
    }
}