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

    public Graph(int V, IMapData mapData)
    {
        v = V;
        adj = new List<int>[v];
        for (int i = 0; i < v; i++)
            adj[i] = new List<int>();

        BuildGraph(mapData);
    }

    public void AddEdge(int v, int w)
    {
        adj[v].Add(w);
        adj[w].Add(v);
        e++;
    }

    private void BuildGraph(IMapData mapData)
    {
        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
            {
                Vector2Int current = new Vector2Int(x, y);
                int currentIndex = y * mapData.Width + x;


                if (y + 1 < mapData.Height) // Check if the cell above is within bounds
                {
                    Vector2Int up = new Vector2Int(x, y + 1);
                    if (!PathfindingAlgorithm.IsMovementBlocked(current, up, mapData))
                    {
                        int upIndex = PathfindingAlgorithm.ConvertToNodeIndex(up, mapData.Width);
                        AddEdge(currentIndex, upIndex);
                    }
                }

                //if (y - 1 >= 0) // Check if the cell below is within bounds
                //{
                //    Vector2Int down = new Vector2Int(x, y - 1);
                //    if (!IsMovementBlocked(current, down, mapData))
                //    {
                //        int downIndex = ConvertToNodeIndex(down, mapData.Width);
                //        mapGraph.AddEdge(currentIndex, downIndex);
                //        BetterDebugDraw(current, down, Color.green, 60f);
                //    }
                //}

                if (x + 1 < mapData.Width) // Check if the cell to the right is within bounds
                {
                    Vector2Int right = new Vector2Int(x + 1, y);
                    if (!PathfindingAlgorithm.IsMovementBlocked(current, right, mapData))
                    {
                        int rightIndex = PathfindingAlgorithm.ConvertToNodeIndex(right, mapData.Width);
                        AddEdge(currentIndex, rightIndex);
                    }
                }

                //if (x - 1 >= 0) // Check if the cell to the left is within bounds
                //{
                //    Vector2Int left = new Vector2Int(x - 1, y);
                //    if (!IsMovementBlocked(current, left, mapData))
                //    {
                //        int leftIndex = ConvertToNodeIndex(left, mapData.Width);
                //        mapGraph.AddEdge(currentIndex, leftIndex);
                //        BetterDebugDraw(current, left, Color.green, 60f);
                //    }
                //}
            }
        }
    }
}
