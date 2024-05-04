using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    public float heuristica;
    public float coste;
    public Vector2 posicion;
    public Node nodoPadre;

    public Node(float heuristica, Vector2 posicion, Node nodoPadre)
    {
        this.heuristica = heuristica;
        this.posicion = posicion;
        this.nodoPadre = nodoPadre;
        if (this.nodoPadre == null) coste = 1;
        else coste = nodoPadre.coste + heuristica;
    }

    public int CompareTo(Node other)
    {
        if (this.heuristica < other.heuristica)
            return -1;
        else if (this.heuristica > other.heuristica)
            return 1;
        else
            return 0;
    }
}

