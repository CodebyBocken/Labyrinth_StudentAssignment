using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Graph
{
    private Grid mapGrid;
    private MapDataAdapter mapData;

    private int v;
    private int e;

    private List<int>[] adj;

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

    public List<int> adjacemt(int v)
    {
        return adj[v];
    }

    public int V()
    {
        // Return number of vertices

        return v;
    }

    public int E()
    {
        // Return number of edges

        return e;
    }
}
