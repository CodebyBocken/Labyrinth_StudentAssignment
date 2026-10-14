using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Authentication;
using Unity.VisualScripting;
using UnityEngine;

public class IndexMinPQ<TKey> : IEnumerable<int> where TKey : IComparable<TKey>
{
    private readonly int maxN;
    private int n;

    private readonly int[] pq;
    private readonly int[] qp;
    private readonly TKey[] keys;

    public IndexMinPQ(int maxN)
    {
        if (maxN < 0)
            throw new ArgumentOutOfRangeException(nameof(maxN));

        this.maxN = maxN;
        n = 0;

        pq = new int[maxN + 1];
        qp = new int[maxN];
        keys = new TKey[maxN];

        for (int i = 0; i < maxN; i++)
            qp[i] = -1;
    }

    public bool IsEmpty() { return n == 0; }

    public int Size() {  return n; }

    public bool Contains(int i)
    {
        ValidateIndex(i);
        return qp[i] != -1;
    }

    public void Insert(int i, TKey key)
    {
        ValidateIndex(i);

        if (Contains(i))
            throw new ArgumentException("Index is already in the priority queue.");

        n++;
        qp[i] = n;
        pq[n] = i;
        keys[i] = key;

        Swim(n);
    }

    public int MinIndex()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Priority queue underflow.");

        return pq[1];
    }

    public TKey MinKey()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Priority queue underflow.");

        return keys[pq[1]];
    }

    public int DelMin()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Priority queue underflow.");

        int min = pq[1];

        Exchange(1, n);
        n--;

        Sink(1);

        qp[min] = -1;
        keys[min] = default;
        pq[n + 1] = -1;

        return min;
    }

    public TKey KeyOf(int i)
    {
        ValidateIndex(i);

        if (!Contains(i))
            throw new InvalidOperationException("Index is not in the priority queue");

        return keys[i];
    }
    
    public void ChangeKey(int i, TKey key)
    {
        ValidateIndex(i);

        if (!Contains(i))
            throw new InvalidOperationException("Index is not in the priority queue");

        keys[i] = key;

        Swim(qp[i]);
        Sink(qp[i]);
    }

    public void DecreaseKey(int i, TKey key)
    {
        ValidateIndex(i);

        if (!Contains(i))
            throw new InvalidOperationException("Index is not in the priority queue");

        int comparison = keys[i].CompareTo(key);

        if (comparison == 0)
            throw new ArgumentException("New key is equal to the current key.");

        if (comparison < 0)
            throw new ArgumentException("New key is greater than the current key.");

        keys[i] = key;
        Swim(qp[i]);
    }

    public void IncreaseKey(int i, TKey key)
    {
        ValidateIndex(i);

        if (!Contains(i))
            throw new InvalidOperationException("Index is not in the priority queue");

        int comparison = keys[i].CompareTo(key);

        if (comparison == 0)
            throw new ArgumentException("New key is equal to the current key.");

        if (comparison > 0)
            throw new ArgumentException("New key is less than the current key.");

        keys[i] = key;
        Sink(qp[i]);
    }

    public void Delete(int i)
    {
        ValidateIndex(i);

        if (!Contains(i))
            throw new InvalidOperationException("Index is not in the priority queue");

        int index = qp[i];

        Exchange(index, n);
        n--;

        Swim(index);
        Sink(index);

        keys[i] = default;
        qp[i] = -1;
    }

    private void ValidateIndex(int i)
    {
        if (i < 0 || i >= maxN)
            throw new ArgumentOutOfRangeException(nameof(i));
    }

    private bool Greater(int i, int j)
    {
        return keys[pq[i]].CompareTo(keys[pq[j]]) > 0;
    }

    private void Exchange(int i, int j)
    {
        int swap = pq[i];
        pq[i] = pq[j];
        pq[j] = swap;

        qp[pq[i]] = i;
        qp[pq[j]] = j;
    }

    private void Swim(int k)
    {
        while (k > 1 && Greater(k / 2, k))
        {
            Exchange(k, k / 2);
            k /= 2;
        }
    }

    private void Sink(int k)
    {
        while (2 * k <= n)
        {
            int j = 2 * k;

            if (j < n && Greater(j, j + 1))
                j++;

            if (!Greater(k, j))
                break;

            Exchange(k, j);
            k = j;
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        var copy = new IndexMinPQ<TKey>(maxN);

        for (int i = 1; i <= n; i++)
            copy.Insert(pq[i], keys[pq[i]]);

        while (!copy.IsEmpty())
            yield return copy.DelMin();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
