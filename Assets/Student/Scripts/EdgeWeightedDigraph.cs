using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;
public class EdgeWeightedDigraph
{
    private int v;
    private int e;
    private int height;
    private int width;

    public int V() { return v; }
    public int E() { return e; }

    private List<DirectedEdge>[] adj;
    public IEnumerable<DirectedEdge> adjacent(int v) { return adj[v]; }

    public EdgeWeightedDigraph(int V, IMapData mapData)
    {
        v = V;
        height = mapData.Height;
        width = mapData.Width;

        adj = new List<DirectedEdge>[v];
        for (int i = 0; i < v; i++)
            adj[i] = new List<DirectedEdge>();

        BuildGraph(mapData);
    }

    public void AddDirectedEdge(DirectedEdge diEdge)
    {
        adj[diEdge.From()].Add(diEdge);
        e++;
    }

    private void BuildGraph(IMapData mapData)
    {

        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
            {
                Vector2Int current = new Vector2Int(x, y);
                int from = ConvertToIndex(x, y);

                // Walls

                if (y + 1 < mapData.Height) // UP
                {
                    Vector2Int upNeighbor = new Vector2Int(x, y + 1);

                    if (!PathfindingAlgorithm.IsMovementBlocked(current, upNeighbor, mapData))
                    {
                        float cost = mapData.GetHorizontalWallCost(x, upNeighbor.y);
                        
                        int to = ConvertToIndex(upNeighbor.x, upNeighbor.y);
                        DirectedEdge edge = new DirectedEdge(from, to, cost);
                        AddDirectedEdge(edge);
                    
                    }
                }

                if (y - 1 >= 0) // DOWN
                {
                    Vector2Int downNeighbor = new Vector2Int(x, y - 1);

                    if (!PathfindingAlgorithm.IsMovementBlocked(current, downNeighbor, mapData))
                    {
                        float cost = mapData.GetHorizontalWallCost(x, y);
                        
                        int to = ConvertToIndex(downNeighbor.x, downNeighbor.y);
                        DirectedEdge edge = new DirectedEdge(from, to, cost);
                        AddDirectedEdge(edge);
                        
                    }
                }

                if (x + 1 < mapData.Width) // RIGHT
                {
                    Vector2Int rightNeighbor = new Vector2Int(x + 1, y);

                    if (!PathfindingAlgorithm.IsMovementBlocked(current, rightNeighbor, mapData))
                    {
                        float cost = mapData.GetVerticalWallCost(rightNeighbor.x, y);
                        
                        int to = ConvertToIndex(rightNeighbor.x, rightNeighbor.y);
                        DirectedEdge edge = new DirectedEdge(from, to, cost);
                        AddDirectedEdge(edge);
                        
                    }
                }

                if (x - 1 >= 0) // LEFT
                {
                    Vector2Int leftNeighbor = new Vector2Int(x - 1, y);

                    if (!PathfindingAlgorithm.IsMovementBlocked(current, leftNeighbor, mapData))
                    {
                        float cost = mapData.GetVerticalWallCost(x, y);
                        
                        int to = ConvertToIndex(leftNeighbor.x, leftNeighbor.y);
                        DirectedEdge edge = new DirectedEdge(from, to, cost);
                        AddDirectedEdge(edge);

                    }
                }

                // Jumps

                foreach(JumpConnection jump in mapData.GetJumpsFrom(new Vector2Int(x, y)))
                {
                    if (!IsInsideMap(jump.To.x, jump.To.y))
                        continue;

                    int jumpIndex = ConvertToIndex(jump.To.x, jump.To.y);

                    DirectedEdge edge = new DirectedEdge(from, jumpIndex, jump.Cost);
                    AddDirectedEdge(edge);
                }
            }
        }

    }

    private int ConvertToIndex(int x, int y)
    {
        return (y * width) + x;
    }
    private bool IsInsideMap(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
