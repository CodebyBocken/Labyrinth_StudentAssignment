using UnityEngine;

public class DirectedEdge
{
    private int v;
    private int w;
    private double weight;

    public int From() { return v; }
    public int To () { return w; }
    public double Weight(){ return weight; }

    public DirectedEdge(int v, int w, double weight)
    {
        this.v = v;
        this.w = w;
        this.weight = weight;
    }
}
