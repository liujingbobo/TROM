using System;

public class PriorityQueue<T> where T : IComparable<T>
{
    private Heap<T> heap;

    public PriorityQueue(bool isMinPriorityQueue = true)
    {
        this.heap = new Heap<T>(isMinPriorityQueue);
    }

    public void Enqueue(T element)
    {
        heap.Add(element);
    }

    public T Dequeue()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Queue is empty");
        }

        return heap.Remove();
    }

    public T Peek()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Queue is empty");
        }

        return heap.Peek();
    }

    public int Count => heap.Count;

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }
    
    public bool Contains(T element)
    {
        return heap.Contains(element);
    }
}