using System;
using System.Collections.Generic;
using UnityEngine;

public class Kruskal : MonoBehaviour
{
    struct node
    {
        public int parent;
        public int rank;
        public node(int v1, int v2) : this()
        {
            parent = v1;
            rank = v2;
        }
    };
    struct Edge
    {
        public int wt;
        public int src;
        public int dst;
       

        public Edge(int v1, int v2, int v3) : this()
        {
            wt = v1;
            src = v2;
            dst = v3;
        }
    };

    List<node> parent = new List<node>();
    List<(int, int, int)> final = new List<(int, int, int)>();

    //find the parent of v (works recrusively) 
    int Find(int v)
    {
        var noeud = parent[v]; 
        if (noeud.parent == -1)
            return v;

        //we need to fetch  c# x-x
        noeud.parent = Find(parent[v].parent);
        return noeud.parent; //Path Compression
    }

    //unite 2 champs 
    void Union(int a, int b )
    {
        a = Find(a);
        b = Find(b);

        //UNION by RANK
        var noeud = parent[b];
        var noeud2 = parent[a];

        if (parent[a].rank > parent[b].rank)
        {  //fromP has higher rank
            noeud.parent = a;
            parent[b] = noeud;
        }
        else if (parent[a].rank < parent[b].rank)
        {  //toP has higher rank
            noeud2.parent = b;
            parent[a] = noeud2;
        }
        else
        {
            //Both have same rank and so anyone can be made as parent
            noeud2.parent = b;
            noeud.rank += 1;      //Increase rank of parent

            parent[a] = noeud2;
            parent[b] = noeud;
        }
    }

    //check cyclic wela le
    bool isCyclic(List<Edge> tupleList)
    {
        foreach (var tuple in tupleList)
        {
            int fromP = Find(tuple.src);       //find parent of subset
            int toP = Find(tuple.dst);

            if (fromP == toP)
                return true;

            //union 
            Union(fromP, toP);
        }

        //end of check
        return false;

    }

    void Krus(List<(int, int, int)> tupleList, int V, int E)
    {
        tupleList.Sort();

        //foreach (var x in tupleList)
        //{
        //    Debug.Log(x.ToString());
        //}


        int i = 0, j = 0;
        while (i < V - 1 && j < E)
        {
            int fromP = Find(tupleList[j].Item2); //FIND absolute parent of subset
            int toP = Find(tupleList[j].Item3);
            print("skiped" + fromP + toP);
            if (fromP == toP)
            { ++j;  continue; }

            //UNION operation
            Union(fromP, toP);   //UNION of 2 sets
            final.Add(tupleList[j]);
            ++i;
        }
    }

    private int Comparator(Edge x, Edge y)
    {
          return x.wt.CompareTo( y.wt);
    }

    void Result(List<(int, int, int)> tupleList)
    {
        print(tupleList.Count.ToString());
        foreach(var edge in tupleList)
        {
            print($"source is {edge.Item2} , desitantion is {edge.Item3} , weight is {edge.Item1}");
        }
    }

    private void Start()
    {
        int E=3; // nb of Edges 
        int V=3;	//No of vertices (0 to V-1)


        for(int i = 0; i <= 3; i++)
        {
            parent.Add(new node(-1,0));
        }

        var tupleList = new List<(int,int,int)>
        {
            //for (int i = 0; i < E; ++i)
            //{
            //    int from, to;
            //    //get from, to 

            //    tupleList.Add((from,to));
            //}
           (4,0, 2),
           (7,0, 1),
           (5,0, 3)
        };


        Krus(tupleList,V,E);
        Result(final);

        //if (isCyclic(tupleList))
        //    print("True");
        //else
        //    print("False");

    }
}
