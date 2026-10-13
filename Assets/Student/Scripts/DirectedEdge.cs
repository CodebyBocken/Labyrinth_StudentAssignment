using UnityEngine;

public class DirectedEdge
{
    private int v;
    private int w;
    private double weight;

    public int From(int v) { return v; }
    public int To (int w) { return w; }
    public double getWeight(){ return weight; }

    public DirectedEdge(int v, int w, double weight)
    {
        this.v = v;
        this.w = w;
        this.weight = weight;
    }
}
