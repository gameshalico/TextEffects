using System;
using System.Runtime.CompilerServices;

namespace TextEffects.Common
{
    public readonly struct PooledHandle<TItem> : IEquatable<PooledHandle<TItem>> where TItem : PooledItem<TItem>, new()
    {
        private readonly PooledItem<TItem> _item;
        public readonly ushort Version;

        public PooledHandle(PooledItem<TItem> item)
        {
            _item = item;
            Version = item.Version;
        }

        public void Return()
        {
            if (!IsValid)
                throw new InvalidOperationException("Handle is invalid");
            PooledItem<TItem>.Return((TItem)_item);
        }

        public TItem Item
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (IsValid)
                    return (TItem)_item;

                throw new InvalidOperationException("Handle is invalid");
            }
        }

        public bool TryGetItem(out TItem item)
        {
            if (IsValid)
            {
                item = (TItem)_item;
                return true;
            }

            item = null;
            return false;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _item != null && _item.Version == Version;
        }

        public static PooledHandle<TItem> Invalid => new(null);

        public static implicit operator PooledHandle<TItem>(TItem item)
        {
            return new PooledHandle<TItem>(item);
        }

        public static implicit operator TItem(PooledHandle<TItem> handle)
        {
            return handle.Item;
        }

        public static bool operator ==(PooledHandle<TItem> a, PooledHandle<TItem> b)
        {
            return a._item == b._item && a.Version == b.Version;
        }

        public static bool operator !=(PooledHandle<TItem> a, PooledHandle<TItem> b)
        {
            return a._item != b._item || a.Version != b.Version;
        }

        public override bool Equals(object obj)
        {
            return obj is PooledHandle<TItem> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_item, Version);
        }

        public bool Equals(PooledHandle<TItem> other)
        {
            return Equals(_item, other._item) && Version == other.Version;
        }
    }
}