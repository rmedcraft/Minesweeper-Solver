// BoardSolver.cs contains the algorithm for solving the board by itself
// Minesweeper.cs contains all the game logic for minesweeper
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class BoardSolver : MonoBehaviour {
    Node start;

    Graph graph;
    GraphView graphView;
    Queue<Node> frontierNodes;
    List<Node> exploreNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploreColor = Color.blue;

    public bool isComplete;
    public int iterations;

    public void Init(Graph graph, GraphView graphView, Node start, Node goal) {
        if (start == null || goal == null || graphView == null || graph == null) {
            Debug.LogWarning("BoardSolver error: Missing components.");
            return;
        }
        if (start.nodeType == NodeType.mine || goal.nodeType == NodeType.mine) {
            Debug.LogWarning("BoardSolver error: Make sure start and goal nodes are open");
            return;
        }

        this.graph = graph;
        this.graphView = graphView;
        this.start = start;

        frontierNodes = new Queue<Node>();
        frontierNodes.Enqueue(start);
        exploreNodes = new List<Node>();

        for (int r = 0; r < graph.GetWidth(); r++) {
            for (int c = 0; c < graph.GetHeight(); c++) {
                this.graph.nodes[r, c].Reset();
            }
        }

        ShowColors(graphView, start, goal);

        isComplete = false;
        iterations = 0;
    }

    public void ShowColors(GraphView graphView, Node start, Node goal) {
        if (graphView == null || start == null || goal == null) {
            return;
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (frontierNodes != null) {
            graphView.ColorNodes(frontierNodes.ToList(), frontierColor);
        }
        if (exploreNodes != null) {
            graphView.ColorNodes(exploreNodes, exploreColor);
        }

        if (startNodeView != null) {
            startNodeView.ColorNode(startColor);
        } else {
            Debug.LogWarning("StartNodeView does not exist");
        }
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f) {
        yield return null;
        while (!isComplete) {

        }
    }
}
