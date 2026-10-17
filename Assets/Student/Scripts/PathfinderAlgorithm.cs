using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class PathfindingAlgorithm
{
    // TODO: Implement the pathfinding assignment here.
    // Find the lowest-cost path from start to goal using the map dimensions, wall costs and directed jumps exposed by IMapData.
    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        // Most of your solution should be implemented in this method.

        //Graph mapGraph = new Graph(mapData.Width * mapData.Height);
        EdgeWeightedDigraph ewDiGraph = new EdgeWeightedDigraph(mapData.Width * mapData.Height, mapData);

        int goalIndex = ConvertToNodeIndex(goal, mapData.Width);
        int startIndex = ConvertToNodeIndex(start, mapData.Width);

        WeightCostDebugging(ewDiGraph);

        //BreadthFirstSearch bfs = new BreadthFirstSearch(mapGraph, startIndex, mapData.Width);
        DijkstrasAlgorithm dijkstra = new DijkstrasAlgorithm(ewDiGraph, startIndex, mapData.Width);

        //List<Vector2Int> path = bfs.GetPath(goalIndex);
        List<Vector2Int> path = dijkstra.GetPath(goalIndex);

        return path;

        Debug.LogWarning("FindShortestPath is not implemented.");
        return null;
    }

    public static bool IsMovementBlocked(Vector2Int from, Vector2Int to, IMapData mapData)
    {
        if (mapData.TryGetJumpCost(from, to, out _))
            return false;

        int deltaX = to.x - from.x;
        int deltaY = to.y - from.y;

        if (Mathf.Abs(deltaX) + Mathf.Abs(deltaY) != 1)
            return true;

        if (deltaX != 0)
        {
            int wallX = deltaX > 0 ? to.x : from.x;
            float wallCost = mapData.GetVerticalWallCost(wallX, from.y);
            return float.IsPositiveInfinity(wallCost) || wallCost >= float.MaxValue;
        }

        int wallY = deltaY > 0 ? to.y : from.y;
        float horizontalWallCost = mapData.GetHorizontalWallCost(from.x, wallY);
        return float.IsPositiveInfinity(horizontalWallCost) || horizontalWallCost >= float.MaxValue;
    }

    private static void BetterDebugDraw(Vector2Int from, Vector2Int to, Color color, float duration)
    {
        Vector3 start = new Vector3(from.x + 0.5f, 0, from.y + 0.5f);
        Vector3 end = new Vector3(to.x + 0.5f, 0, to.y + 0.5f);

        Debug.DrawLine(start, end, color, duration);
    }

    private static void WeightCostDebugging(EdgeWeightedDigraph ewDiGraph)
    {
        int edgeCount = 0;
        int nonUnitWeights = 0;

        for (int v = 0; v < ewDiGraph.V(); v++)
        {
            foreach (DirectedEdge edge in ewDiGraph.adjacent(v))
            {
                edgeCount++;

                if (edge.Weight() != 1.0)
                {
                    nonUnitWeights++;

                    Debug.Log(
                        $"Weight != 1: {edge.From()} -> {edge.To()}, " +
                        $"weight = {edge.Weight()}"
                    );
                }
            }
        }
    }

    public static int ConvertToNodeIndex(Vector2Int node, int width)
    {
        return (node.y * width) + node.x;
    }

    public static Vector2Int ConvertToNode(int nodeIndex, int width)
    {
        int x = nodeIndex % width;
        int y = nodeIndex / width;

        return new Vector2Int(x, y);
    }
}
