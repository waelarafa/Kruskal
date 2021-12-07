// C# Code for above approach
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
class K : MonoBehaviour
{
	bool simulate=false;

	public static K _instance;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);

			//Rest of your Awake code

		}
		else
		{
			Destroy(this);
		}
	}


	// A class to represent a graph edge
	class Edge : IComparable<Edge>
	{
		public int src, dest, weight;
		public Vector3 poseS, poseD;
		
		public int CompareTo(Edge compareEdge)
		{
			return this.weight
				- compareEdge.weight;
		}
	}

	// A class to represent
	// a subset for union-find
	public class subset
	{
		public int parent, rank;
	};

	int V, E; // V-> no. of vertices & E->no.of edges
	Edge[] edge; // collection of all edges

	// Creates a graph with V vertices and E edges
	 K(int v, int e)
	{
		V = v;
		E = e;
		edge = new Edge[E];
		for (int i = 0; i < e; ++i)
			edge[i] = new Edge();
		return;
	}

	// A utility function to find set of an element i
	// (uses path compression technique)
	int find(subset[] subsets, int i)
	{
		// find root and make root as
		// parent of i (path compression)
		if (subsets[i].parent != i)
			subsets[i].parent
				= find(subsets, subsets[i].parent);

		return subsets[i].parent;
	}

	// A function that does union of
	// two sets of x and y (uses union by rank)
	void Union(subset[] subsets, int x, int y)
	{
		int xroot = find(subsets, x);
		int yroot = find(subsets, y);

		// Attach smaller rank tree under root of
		// high rank tree (Union by Rank)
		if (subsets[xroot].rank < subsets[yroot].rank)
			subsets[xroot].parent = yroot;
		else if (subsets[xroot].rank > subsets[yroot].rank)
			subsets[yroot].parent = xroot;

		// If ranks are same, then make one as root
		// and increment its rank by one
		else
		{
			subsets[yroot].parent = xroot;
			subsets[xroot].rank++;
		}
	}

	// The main function to construct MST
	// using Kruskal's algorithm

	void KruskalMST()
	{
		// This will store the
		// resultant MST
		Edge[] result = new Edge[V];
		int e = 0; // An index variable, used for result[]
		int i = 0; // An index variable, used for sorted edges
		for (i = 0; i < V; ++i)
			result[i] = new Edge();

		// Step 1: Sort all the edges in non-decreasing
		// order of their weight. If we are not allowed
		// to change the given graph, we can create
		// a copy of array of edges
		Array.Sort(edge);

		// Allocate memory for creating V ssubsets
		subset[] subsets = new subset[V];
		for (i = 0; i < V; ++i)
			subsets[i] = new subset();

		// Create V subsets with single elements
		for (int v = 0; v < V; ++v)
		{
			subsets[v].parent = v;
			subsets[v].rank = 0;
		}

		i = 0; // Index used to pick next edge

		// Number of edges to be taken is equal to V-1
		while (e < V - 1)
		{
			// Step 2: Pick the smallest edge. And increment
			// the index for next iteration
			Edge next_edge = new Edge();
			next_edge = edge[i++];

			int x = find(subsets, next_edge.src);
			int y = find(subsets, next_edge.dest);

			// If including this edge does't cause cycle,
			// include it in result and increment the index
			// of result for next edge
			if (x != y)
			{
				GameObject myLine = new GameObject();
				myLine.AddComponent<LineRenderer>();
				myLine.transform.position = next_edge.poseS;
				LineRenderer lr = myLine.GetComponent<LineRenderer>();
				lr.SetPosition(0, next_edge.poseS);
				lr.SetPosition(1, next_edge.poseD);
				Gradient gradient = new Gradient();
				gradient.SetKeys(
					new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
					new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0.0f), new GradientAlphaKey(0.5f, 1.0f) }
				);
				lr.colorGradient = gradient;
				lr.material = new Material(Shader.Find("Sprites/Default"));


				result[e++] = next_edge;
				Union(subsets, x, y);
			}

		}
		// print the contents of result[] to display
		// the built MST
		print("Following are the edges in "
						+ "the constructed MST");

		int minimumCost = 0;
		for (i = 0; i < e; ++i)
		{
			print(result[i].src + " -- "
							+ result[i].dest
							+ " == " + result[i].weight);
			minimumCost += result[i].weight;
		}

		print("Minimum Cost Spanning Tree"
						+ minimumCost);

	}


	// Driver Code
	public void Init(int v, int E, List<(int, int, int, Vector3, Vector3)> edges)
	{



		K graph = new K(v, E);

		int i = 0;
		foreach (var edge in edges)
		{
			graph.edge[i].src = edge.Item1;
			graph.edge[i].dest = edge.Item2;
			graph.edge[i].weight = edge.Item3;
			graph.edge[i].poseS = edge.Item4;
			graph.edge[i].poseD = edge.Item5;
			i++;
		}


		//// add edge 0-1
		//graph.edge[0].src = 0;
		//graph.edge[0].dest = 1;
		//graph.edge[0].weight = 10;

		//// add edge 0-2
		//graph.edge[1].src = 1;
		//graph.edge[1].dest = 2;
		//graph.edge[1].weight = 6;

		//// add edge 0-3
		//graph.edge[2].src = 2;
		//graph.edge[2].dest = 0;
		//graph.edge[2].weight = 5;

		//// add edge 1-3
		//graph.edge[3].src = 1;
		//graph.edge[3].dest = 3;
		//graph.edge[3].weight = 15;

		//// add edge 2-3
		//graph.edge[4].src = 2;
		//graph.edge[4].dest = 3;
		//graph.edge[4].weight = 4;

		// Function call
		graph.KruskalMST();
	}

	public void Sim()
	{
		simulate = true;
	}

}


