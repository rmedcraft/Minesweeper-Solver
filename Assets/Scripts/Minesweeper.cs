// Minesweeper.cs contains all the game logic for minesweeper
// BoardSolver.cs contains the algorithm for solving the board by itself
using System.Collections.Generic;
using UnityEngine;

public class Minesweeper : MonoBehaviour {
    Graph graph;
    GraphView graphView;
    UIManager ui;
    BoardSolver boardSolver;
    public bool hasClicked = false; // determines if the player has clicked yet to generate the board on the first click
    public bool isPlaying = true; // doesnt allow user input if the game isnt going on
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Graph graph, GameController gameController) {
        this.graph = graph;
        graphView = graph.GetComponent<GraphView>();
        this.ui = gameController.ui;
        this.boardSolver = gameController.boardSolver;
    }

    // reveals a given node n
    public void RevealNode(Node n) {
        // change viewtype & color
        NodeView nview = graphView.nodeViews[n.xIndex, n.yIndex];
        if (nview.viewType == ViewType.flagged) {
            return;
        }


        // ensures the first click isnt a mine
        if (!hasClicked) {
            graph.OnFirstClick(n);
            hasClicked = true;
        }


        nview.viewType = ViewType.open;
        graphView.ColorNode(n);

        // display the number of bordering mines if the bordering mines is >0
        int mines = n.CountMines();

        // player clicked a mine & lost
        if (n.nodeType == NodeType.mine) {
            isPlaying = false;
            boardSolver.isSolving = false;
            ui.resultText.text = "You Lose!";
            RevealMines();
            return;
        }
        // player clicked a non-mine & keeps playing
        if (mines != 0 && n.nodeType != NodeType.mine) {
            nview.DrawText(mines.ToString());

            // if the node isnt already in the frontier nodes for the algorithm, put it in there!
            if (!boardSolver.frontierNodes.Contains(n)) {
                boardSolver.frontierNodes.Enqueue(n);
            }
        } else if (mines == 0 && n.nodeType == NodeType.open) {
            // reveal mines surrounding this square
            RevealNeighbors(n);
        }


        if (CheckWin()) {
            ui.resultText.text = "You Won!";
        }
    }

    // returns true if the player won the game, returns false if the game is still going
    public bool CheckWin() {
        foreach (Node n in graph.nodes) {
            NodeView nview = graphView.nodeViews[n.xIndex, n.yIndex];
            if (n.nodeType == NodeType.open && nview.viewType != ViewType.open) {
                return false;
            }
        }
        return true;
    }

    public void RevealMines() {
        List<Node> mineList = new List<Node>();
        foreach (Node n in graph.nodes) {
            if (n.nodeType == NodeType.mine) {
                mineList.Add(n);
            }
        }

        graphView.ColorNodes(mineList, graphView.mineColor);
    }

    public void FlagNode(Node n) {
        NodeView nview = graphView.nodeViews[n.xIndex, n.yIndex];
        if (nview.viewType == ViewType.open) {
            return;
        }
        nview.viewType = nview.viewType == ViewType.closed ? ViewType.flagged : ViewType.closed; // toggle between closed and flagged
        nview.DrawText(nview.viewType == ViewType.flagged ? "F" : "");
    }

    // When you click on a open cell, if the number of flags matches the number of mines, clear all cells bordering the clicked node
    public void ClearFlagged(Node n) {
        int flagCt = 0;
        foreach (Node neighbor in n.neighbors) {
            NodeView nview = graphView.nodeViews[neighbor.xIndex, neighbor.yIndex];
            if (nview.viewType == ViewType.flagged) {
                flagCt++;
            }
        }
        if (flagCt == n.CountMines()) {
            RevealNeighbors(n);
        }
    }

    // reveals every closed neighbor to a given node n
    public void RevealNeighbors(Node n) {
        foreach (Node neighbor in n.neighbors) {
            NodeView nview = graphView.nodeViews[neighbor.xIndex, neighbor.yIndex];
            if (nview.viewType == ViewType.flagged || nview.viewType == ViewType.open) {
                continue;
            }
            RevealNode(neighbor);
        }
    }

    public bool GetPlaying() {
        return isPlaying;
    }
}
