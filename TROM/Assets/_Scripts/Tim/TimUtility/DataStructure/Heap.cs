using System;
using System.Collections.Generic;

public class Heap<T> where T : IComparable<T>
{
    private List<T> data;
    private bool IsMinHeap;

    public Heap(bool isMinHeap = true)
    {
        this.data = new List<T>();
        this.IsMinHeap = isMinHeap;
    }

    private int ParentIndex(int childIndex) => (childIndex - 1) / 2;

    private int LeftChildIndex(int parentIndex) => 2 * parentIndex + 1;

    private int RightChildIndex(int parentIndex) => 2 * parentIndex + 2;

    private bool HasParent(int childIndex) => ParentIndex(childIndex) >= 0;

    private bool HasLeftChild(int parentIndex) => LeftChildIndex(parentIndex) < data.Count;

    private bool HasRightChild(int parentIndex) => RightChildIndex(parentIndex) < data.Count;

    private T Parent(int childIndex) => data[ParentIndex(childIndex)];

    private T LeftChild(int parentIndex) => data[LeftChildIndex(parentIndex)];

    private T RightChild(int parentIndex) => data[RightChildIndex(parentIndex)];

    private void Swap(int indexOne, int indexTwo)
    {
        T temp = data[indexOne];
        data[indexOne] = data[indexTwo];
        data[indexTwo] = temp;
    }

    public int Count => data.Count;

    public T Peek()
    {
        if (data.Count == 0)
        {
            throw new InvalidOperationException("Heap is empty");
        }

        return data[0];
    }

    public void Add(T element)
    {
        data.Add(element);
        HeapifyUp();
    }

    public T Remove()
    {
        if (data.Count == 0)
        {
            throw new InvalidOperationException("Heap is empty");
        }

        T element = data[0];
        data[0] = data[data.Count - 1];
        data.RemoveAt(data.Count - 1);

        HeapifyDown();

        return element;
    }

    private void HeapifyUp()
    {
        int index = data.Count - 1;

        while (HasParent(index) && Compare(data[index], Parent(index)) < 0)
        {
            Swap(index, ParentIndex(index));
            index = ParentIndex(index);
        }
    }

    private void HeapifyDown()
    {
        int index = 0;

        while (HasLeftChild(index))
        {
            int smallerChildIndex = LeftChildIndex(index);

            if (HasRightChild(index) && Compare(RightChild(index), LeftChild(index)) < 0)
            {
                smallerChildIndex = RightChildIndex(index);
            }

            if (Compare(data[index], data[smallerChildIndex]) < 0)
            {
                break;
            }

            Swap(index, smallerChildIndex);
            index = smallerChildIndex;
        }
    }

    public bool Contains(T element)
    {
        return data.Contains(element);
    }
    
    private int Compare(T first, T second)
    {
        return IsMinHeap ? first.CompareTo(second) : second.CompareTo(first);
    }
}