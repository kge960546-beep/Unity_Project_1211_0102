using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SimplePriorityQueue<T> where T : IComparable<T>
{
    // NOTE: This class does not implement versioning feature observed in .NET collections.
    // NOTE: This class use constant value 4 as an arity.
    private const int Arity = 4;
    private const int Log2Arity = 2;
    private const int MaxCapacity = int.MaxValue - 128; // cannot guarantee exact size before .NET 6 (Array.MaxLength). Just use a heuristic value.


    private T[] _tree;
    private int _count;

    public int Count => _count;
    public int Capacity => _tree.Length;

    private void GrowCapacity(int minimumCapacity)
    {
        const int GrowthFactor = 2;
        const int MinimumGrowth = Arity;
        const int AbsoluteMinimumCapacity = Arity << 4;

        int newCapacity = Mathf.Clamp(_tree.Length * GrowthFactor, 0, MaxCapacity);
        newCapacity = Mathf.Max(AbsoluteMinimumCapacity, _tree.Length + MinimumGrowth, minimumCapacity, newCapacity);

        Array.Resize(ref _tree, newCapacity);
    }


    public SimplePriorityQueue()
    {
        _tree = Array.Empty<T>();
    }

    public SimplePriorityQueue(int initialCapacity)
    {
        if (initialCapacity < 0) throw new ArgumentOutOfRangeException("Capacity should not be a negative number.");
        _tree = new T[initialCapacity];
    }

    public void Enqueue(T element)
    {
        if (Capacity == Count) GrowCapacity(Count + 1);
        _count++;
        PushToTree(element);
    }

    public T Peek()
    {
        if (0 == _count) throw new InvalidOperationException("Attempted to peek at an empty queue.");
        return _tree[0];
    }

    public T Dequeue()
    {
        if (0 == _count) throw new InvalidOperationException("Attempted to dequeue from an empty queue.");
        T result = _tree[0];
        PopFromTree();
        return result;
    }

    public void Clear()
    {
        Array.Clear(_tree, 0, Count);
        _count = 0;
    }

    private void PushToTree(T element)
    {
        T[] tree = _tree;
        int targetIdx = _count - 1;

        while (targetIdx > 0)
        {
            int parentIdx = GetParentIdx(targetIdx);
            T parent = tree[parentIdx];
            if (Comparer<T>.Default.Compare(element, parent) < 0)
            {
                tree[targetIdx] = parent;
                targetIdx = parentIdx;
            }
            else break;
        }
        tree[targetIdx] = element;
    }

    private void PopFromTree()
    {
        int lastElementIdx = _count - 1;

        if (lastElementIdx > 0)
        {
            T last = _tree[lastElementIdx];
            PropagatePopping(last, 0);
        }

        _count--;
        _tree[lastElementIdx] = default;
    }

    private void PropagatePopping(T element, int sourceIdx)
    {
        T[] tree = _tree;
        int count = _count;

        // need rewrite
        int comparingChildIdx;
        while ((comparingChildIdx = GetFirstChildIdx(sourceIdx)) < count)
        {
            int childIdxUpperBound = Math.Min(comparingChildIdx + Arity, count);

            int minimumChildIdx = comparingChildIdx;
            T minimumChild = tree[comparingChildIdx];

            while (++comparingChildIdx < childIdxUpperBound)
            {
                T comparingChild = tree[comparingChildIdx];
                if (Comparer<T>.Default.Compare(comparingChild, minimumChild) < 0)
                {
                    minimumChild = comparingChild;
                    minimumChildIdx = comparingChildIdx;
                }
            }

            if (Comparer<T>.Default.Compare(element, minimumChild) <= 0) break;
            
            tree[sourceIdx] = minimumChild;
            sourceIdx = minimumChildIdx;
        }
        tree[sourceIdx] = element;
    }

    private static int GetParentIdx(int targetIdx) => (targetIdx - 1) >> Log2Arity;
    private static int GetFirstChildIdx(int targetIdx) => (targetIdx << Log2Arity) + 1;
}
