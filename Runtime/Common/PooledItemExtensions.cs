namespace TextEffects.Common
{
    public static class PooledItemExtensions
    {
        public static PooledHandle<T> Handle<T>(this T item) where T : PooledItem<T>, new()
        {
            return new PooledHandle<T>(item);
        }
    }
}