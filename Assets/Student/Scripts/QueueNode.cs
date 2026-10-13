using System;

public class QueueNode : IComparable<QueueNode>
{
    public int Index { get; }
    public double Distance { get; }

    public QueueNode (int index, double distance)
    {
        Index = index; 
        Distance = distance;
    }

    public int CompareTo(QueueNode other)
    {
        return Distance.CompareTo(other.Distance);
    }
}
