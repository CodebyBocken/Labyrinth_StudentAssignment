using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DijkstrasAlgorithm
{
    private DirectedEdge[] edgeTo;
    private double[] distTo;
    private MyPriorityQueue<QueueNode> pq;

    public DijkstrasAlgorithm(EdgeWeightedDigraph G, int S)
    {
        edgeTo = new DirectedEdge[S];
        distTo = new double[S];
        pq = new MyPriorityQueue<QueueNode>();
    }

    private void Relax()
    {

    }
}
