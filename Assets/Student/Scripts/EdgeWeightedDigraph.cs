using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class EdgeWeightedDigraph
{
    private int v;
    private int e;
    private int height;
    private int width;

    private Vector2Int[] directions;

    public int V() { return v; }
    public int E() { return e; }

    private List<DirectedEdge>[] adj;
    public IEnumerable<DirectedEdge> adjacent(int v) { return adj[v]; }

    public EdgeWeightedDigraph(int V, IMapData mapData)
    {
        v = V;
        height = mapData.Height;
        width = mapData.Width;

        directions = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

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
                int from = ConvertToIndex(x, y);

                // Walls

                if (y + 1 < mapData.Height && !mapData.HasHorizontalWall(x, y)) // UP
                {
                    int up = ConvertToIndex(x, y + 1);

                    float cost = mapData.GetHorizontalWallCost(x, y);
                    AddDirectedEdge(new DirectedEdge(from, up, cost));
                }

                if (y - 1 >= 0 && !mapData.HasHorizontalWall(x, y)) // DOWN
                {
                    int down = ConvertToIndex(x, y - 1);

                    float cost = mapData.GetHorizontalWallCost(x, y);
                    AddDirectedEdge(new DirectedEdge(from, down, cost));
                }

                if (x + 1 >= 0 && !mapData.HasVerticalWall(x, y)) // RIGHT
                {
                    int right = ConvertToIndex(x + 1, y);

                    float cost = mapData.GetVerticalWallCost(x, y);
                    AddDirectedEdge(new DirectedEdge(from, right, cost));
                }

                if (x - 1 < mapData.Width && !mapData.HasVerticalWall(x, y)) // LEFT
                {
                    int left = ConvertToIndex(x - 1, y);

                    float cost = mapData.GetVerticalWallCost(x, y);
                    AddDirectedEdge(new DirectedEdge(from, ConvertToIndex(x - 1, y), cost));
                }

                // Jumps

                foreach(JumpConnection jump in mapData.GetJumpsFrom(new Vector2Int(x, y)))
                {
                    if (!IsInsideMap(jump.To.x, jump.To.y))
                        continue;

                    int jumpIndex = ConvertToIndex(jump.To.x, jump.To.y);

                    AddDirectedEdge(new DirectedEdge(from, jumpIndex, jump.Cost));
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
