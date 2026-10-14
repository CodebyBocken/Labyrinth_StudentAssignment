using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DijkstrasAlgorithm
{
    private DirectedEdge[] edgeTo;
    private double[] distTo;
    private IndexMinPQ<double> pq;

    public DijkstrasAlgorithm(EdgeWeightedDigraph G, int S)
    {
        edgeTo = new DirectedEdge[G.V()];
        distTo = new double[G.V()];
        pq = new IndexMinPQ<double>(G.V());

        for (int v = 0; v < G.V(); v++)
            distTo[v] = double.PositiveInfinity;
        distTo[S] = 0.0;

        pq.Insert(S, 0.0);
        while (!pq.IsEmpty())
        {
            int v = pq.DelMin();
            foreach (DirectedEdge edge in G.adjacent(v))
                Relax(edge);
        }
    }

    private void Relax(DirectedEdge e)
    {
        int v = e.From();
        int w = e.To();

        double newDistance = distTo[v] + e.Weight();
        if (distTo[w] > newDistance)
        {
            distTo[w] = newDistance;
            edgeTo[w] = e;

            if (pq.Contains(w))
                pq.DecreaseKey(w, distTo[w]);
            else
                pq.Insert(w, distTo[w]);
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
