using System;

public abstract class IHeapItem<T> : IComparable<T>
{
    public int HeapIndex{ get; set; }

    public abstract int CompareTo(T other);
}
