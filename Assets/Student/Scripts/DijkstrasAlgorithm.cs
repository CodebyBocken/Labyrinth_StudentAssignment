using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DijkstrasAlgorithm
{
    private DirectedEdge[] edgeTo;
    private double[] distTo;
    private MyPriorityQueue<QueueNode> pq;

    public DijkstrasAlgorithm(EdgeWeightedDigraph G, int S)
    {
        edgeTo = new DirectedEdge[G.V()];
        distTo = new double[G.V()];
        pq = new MyPriorityQueue<QueueNode>();

        for (int v = 0; v < G.V(); v++)
            distTo[v] = double.PositiveInfinity;
        distTo[S] = 0.0;

        pq.Enqueue(new QueueNode(S, 0.0));
        while (!pq.IsEmpty)
        {
            QueueNode current = pq.Dequeue();
            int v = current.Index;

            if (current.Distance > distTo[v])
                continue;

            foreach (DirectedEdge e in G.adjacent(v))
                Relax(e);
        }
    }

    private void Relax(DirectedEdge e)
    {
        int v = e.From();
        int w = e.To();

        double newDistance = distTo[v] + e.getWeight();

        if (newDistance < distTo[w])
        {
            distTo[w] = newDistance;
            edgeTo[w] = e;

            pq.Enqueue(new QueueNode(w, newDistance));
        }
    }

    public double DistTo(int v)
    {
        return distTo[v];
    }

    public bool HasPathTo(int v)
    {
        return distTo[v] < double.PositiveInfinity;
    }
}
