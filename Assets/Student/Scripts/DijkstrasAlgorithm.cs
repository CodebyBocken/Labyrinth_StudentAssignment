using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DijkstrasAlgorithm
{
    private int s;
    private int width;
    private DirectedEdge[] edgeTo;
    private double[] distTo;
    private IndexMinPQ<double> pq;

    public DijkstrasAlgorithm(EdgeWeightedDigraph G, int S, int width)
    {
        s = S;
        this.width = width;

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

        if (distTo[w] > distTo[v] + e.Weight())
        {
            distTo[w] = distTo[v] + e.Weight();
            edgeTo[w] = e;

            if (pq.Contains(w))
                pq.DecreaseKey(w, distTo[w]);
            else
                pq.Insert(w, distTo[w]);
        }
    }

    public List<Vector2Int> GetPath(int goal)
    {
        if (!HasPathTo(goal))
            return new List<Vector2Int>();

        Stack<int> path = new Stack<int>();
        int current = goal;

        while (current != s)
        {
            path.Push(current);

            DirectedEdge edge = edgeTo[current];
            current = edge.From();
        }

        path.Push(s);

        List<Vector2Int> result = new List<Vector2Int>();

        while (path.Count > 0)
        {
            int index = path.Pop();

            result.Add(PathfindingAlgorithm.ConvertToNode(index, width));
        }

        return result;
    }

    public IEnumerable<DirectedEdge> PathTo(int goal)
    {
        if (!HasPathTo(goal)) return null;

        Stack<DirectedEdge> path = new Stack<DirectedEdge>();
        for(DirectedEdge edge = edgeTo[goal]; edge != null; edge = edgeTo[edge.From()])
        {
            path.Push(edge);
        }

        return path;
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
