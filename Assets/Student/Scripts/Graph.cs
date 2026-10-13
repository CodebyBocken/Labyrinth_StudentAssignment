using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Graph
{

    private int v;
    private int e;

    public int V() { return v; }
    public int E() { return e; }

    private List<int>[] adj;
    public IEnumerable<int> adjacent(int v) { return adj[v]; }

    public Graph(int V)
    {
        v = V;
        adj = new List<int>[v];
        for (int i = 0; i < v; i++)
            adj[i] = new List<int>();
    }

    public void AddEdge(int v, int w)
    {
        adj[v].Add(w);
        adj[w].Add(v);
        e++;
    }

    public void AddDirectedEdge(int v, int w)
    {
        adj[v].Add(w);
        e++;
    }

    public void AddWeightEdge(int v, int w, float weight)
    {
        adj[v].Add(w);
        adj[w].Add(v);
        e++;
    }
}
