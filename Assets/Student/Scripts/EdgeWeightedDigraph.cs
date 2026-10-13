using System.Collections.Generic;
using UnityEngine;
public class EdgeWeightedDigraph
{
    private int v;
    private int e;

    public int V() { return v; }
    public int E() { return e; }

    private List<DirectedEdge>[] adj;
    public IEnumerable<DirectedEdge> adjacent(int v) { return adj[v]; }

    public EdgeWeightedDigraph(int V)
    {
        v = V;
        adj = new List<DirectedEdge>[v];
        for (int i = 0; i < v; i++)
            adj[i] = new List<DirectedEdge>();
    }

    public void AddDirectedEdge(DirectedEdge diEdge)
    {
        adj[v].Add(diEdge);
        e++;
    }
}
