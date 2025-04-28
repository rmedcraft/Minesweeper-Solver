// BoardSolver.cs contains the algorithm for solving the board by itself
// Minesweeper.cs contains all the game logic for minesweeper
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class BoardSolver : MonoBehaviour {

    Graph graph;
    GraphView graphView;
    public Queue<Node> frontierNodes;

    public Color frontierColor = Color.magenta;

    public bool isComplete;
    public int iterations;

    public bool isSolving = false;

    Minesweeper minesweeper;

    public void Init(Graph graph, GameController gameController) {
        if (graph == null || gameController == null) {
            Debug.LogWarning("BoardSolver error: Missing components.");
            return;
        }

        this.graph = graph;
        this.graphView = graph.GetComponent<GraphView>();
        this.minesweeper = gameController.minesweeper;

        frontierNodes = new Queue<Node>();
        isComplete = false;

        StartCoroutine(SearchRoutine());
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f) {
        yield return null;


        while (!isComplete) {

            // paused solve
            while (!isSolving) {
                graphView.ColorNodes(frontierNodes.ToList(), graphView.openColor);
                yield return new WaitForSeconds(timeStep);
            }

            if (!minesweeper.hasClicked) {
                minesweeper.RevealNode(graph.nodeList[Random.Range(0, graph.nodeList.Count)]);
                continue;
            }

            Node node = frontierNodes.Dequeue();
            int mines = node.CountMines();

            List<Node> closed = new List<Node>();
            List<Node> open = new List<Node>();
            List<Node> flagged = new List<Node>();
            foreach (Node n in node.neighbors) {
                NodeView nview = graphView.nodeViews[n.xIndex, n.yIndex];
                if (nview.viewType == ViewType.flagged) {
                    flagged.Add(n);
                }
                if (nview.viewType == ViewType.open) {
                    open.Add(n);
                }
                if (nview.viewType == ViewType.closed) {
                    closed.Add(n);
                }
            }

            if (closed.Count + flagged.Count == mines) {
                foreach (Node n in closed) {
                    minesweeper.FlagNode(n);
                }
            } else if (flagged.Count == mines) {
                foreach (Node n in closed) {
                    minesweeper.RevealNode(n);
                }
            } else {
                frontierNodes.Enqueue(node);
            }

            NodeView nodeView = graphView.nodeViews[node.xIndex, node.yIndex];
            nodeView.ColorNode(frontierColor);
            yield return new WaitForSeconds(timeStep);
            nodeView.ColorNode(nodeView.viewType == ViewType.open ? graphView.openColor : graphView.closedColor);
        }
    }
}
