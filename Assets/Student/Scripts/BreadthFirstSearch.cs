using UnityEngine.Rendering.Universal.Internal;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch
{
    private bool[] marked;
    private int[] edgeTo;
    private readonly int s;
    private readonly int width;

    public BreadthFirstSearch(Graph G, int S, int mapWidth)
    {
        marked = new bool[G.V()];
        edgeTo = new int[G.V()];
        this.s = S;
        this.width = mapWidth;

        Bfs(G, S);
    }


    private void Bfs(Graph G, int S)
    {
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(S);
        marked[S] = true;

        while (queue.Count > 0)
        {
            int v = queue.Dequeue();
            
            foreach (var w in G.adjacent(v))
            {
                if (!marked[w])
                {
                    queue.Enqueue(w);
                    marked[w] = true;
                    edgeTo[w] = v;
                }
            }
        }
    }

    public List<Vector2Int> PathToGoal(int goal) // Can also use IEnumerable to iterate over stack without having to use pop.
    {
        if (!marked[goal])
            return new List<Vector2Int>();


        Stack<int> path = new Stack<int>();
        int current = goal;

        while (current != s)
        {
            path.Push(current);
            current = edgeTo[current];
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

    //private IEnumerable<int> PathToGoal(int goal) // Can use this if not using base PathToGoal()
    //{
    //    if (!marked[goal]) return null;

    //    Stack<int> path = new Stack<int>();
    //    for (int current = goal; current != s; current = edgeTo[current])
    //        path.Push(current);
    //    path.Push(s);
    //    return path;
    //}

    //public List<Vector2Int> ConvertingToVectorPath(IEnumerable<int> tempPath)
    //{
    //    List<Vector2Int> path = new List<Vector2Int>();
    //    foreach (int index in tempPath)
    //    {
    //        path.Add(PathfindingAlgorithm.ConvertToNode(index, width));
    //    }
    //    return path;
    //}
}
