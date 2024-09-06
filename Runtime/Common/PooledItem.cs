using System;

namespace TextEffects.Common
{
    public abstract class PooledItem<T> : IDisposable where T : PooledItem<T>, new()
    {
        private static T s_poolRoot;
        private T _next;
        public ushort Version { get; private set; }

        public void Dispose()
        {
            Return(this as T);
        }

        protected virtual void OnRent()
        {
        }

        protected virtual void OnReturn()
        {
        }

        public static T Rent()
        {
            T item;
            if (s_poolRoot == null)
            {
                item = new T();
            }
            else
            {
                item = s_poolRoot;
                s_poolRoot = item._next;
                item._next = null;
            }

            item.OnRent();
            return item;
        }

        public static void Return(T item)
        {
            if (item._next != null)
                throw new InvalidOperationException("The item is already in the pool.");
            item.OnReturn();

            item.Version++;
            if (item.Version == ushort.MaxValue)
                return;

            item._next = s_poolRoot;
            s_poolRoot = item;
        }
    }
}