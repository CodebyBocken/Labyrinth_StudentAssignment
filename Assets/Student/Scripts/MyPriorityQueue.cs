using System;
using System.Collections.Generic;

public class MyPriorityQueue<T> where T : IComparable<T>
{
    private readonly List<T> minHeap;
    public int Count { get { return minHeap.Count; } }
    public bool IsEmpty {  get { return minHeap.Count == 0; } }

    public void Enqueue(T item)
    {
        minHeap.Add(item);
        Swim(minHeap.Count - 1);
    }

    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Priority queue is empty");

        T min = minHeap[0];

        Swap(0, minHeap.Count - 1);
        minHeap.RemoveAt(minHeap.Count - 1);

        if (!IsEmpty)
            Sink(0);

        return min;
    }

    public T peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Priority queue is empty");

        return minHeap[0];
    }

    private void Swim(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;

            if (minHeap[i].CompareTo(minHeap[parent]) >= 0)
                break;

            Swap(i, parent);
            i = parent;
        }
    }

    private void Sink(int i)
    {
        while (2 * i + 1 < minHeap.Count)
        {
            int child = 2 * i + 1;
            int right = child + 1;

            if (right < minHeap.Count && minHeap[right].CompareTo(minHeap[child]) < 0)
            {
                child = right;
            }

            if (minHeap[i].CompareTo(minHeap[child]) <= 0)
                break;

            Swap(i, child);
            i = child;
        }
    }

    private void Swap(int i, int j)
    {
        T temp = minHeap[i];
        minHeap[i] = minHeap[j];
        minHeap[j] = temp;
    }
}
