using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PathfindingAlgorithm
{
    // TODO: Implement the pathfinding assignment here.
    // Find the lowest-cost path from start to goal using the map dimensions, wall costs and directed jumps exposed by IMapData.
    public static List<Vector2Int> FindShortestPath(Vector2Int start, Vector2Int goal, IMapData mapData)
    {
        // Most of your solution should be implemented in this method.

        Graph mapGraph = new Graph(mapData.Width * mapData.Height);
        int goalIndex = ConvertToNodeIndex(goal, mapData.Width);
        int startIndex = ConvertToNodeIndex(start, mapData.Width);

        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
            {
                Vector2Int current = new Vector2Int(x, y);
                int currentIndex = y * mapData.Width + x;

                
                if (y + 1 < mapData.Height) // Check if the cell above is within bounds
                {
                    Vector2Int up = new Vector2Int(x, y + 1);
                    if (!IsMovementBlocked(current, up, mapData))
                    {
                        int upIndex = ConvertToNodeIndex(up, mapData.Width);
                        mapGraph.AddEdge(currentIndex, upIndex);
                        BetterDebugDraw(current, up, Color.green, 60f);
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
                    if (!IsMovementBlocked(current, right, mapData))
                    {
                        int rightIndex = ConvertToNodeIndex(right, mapData.Width);
                        mapGraph.AddEdge(currentIndex, rightIndex);
                        BetterDebugDraw(current, right, Color.green, 60f);
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

        Debug.Log($"Nodes: {mapGraph.V()}, Edges: {mapGraph.E()}");

        BreadthFirstSearch bfs = new BreadthFirstSearch(mapGraph, startIndex, mapData.Width);

        return bfs.PathToGoal(goalIndex);

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
