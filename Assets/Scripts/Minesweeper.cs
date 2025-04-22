// Minesweeper.cs contains all the game logic for minesweeper
// BoardSolver.cs contains the algorithm for solving the board by itself
using UnityEngine;

public class Minesweeper : MonoBehaviour {
    Graph graph;
    GraphView graphView;

    bool hasClicked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Graph graph) {
        this.graph = graph;
        graphView = graph.GetComponent<GraphView>();
    }

    public void RevealNode(Node n) {
        // change viewtype & color
        NodeView nview = graphView.nodeViews[n.xIndex, n.yIndex];
        if (nview.viewType == ViewType.flagged) {
            return;
        }


        // ensures the first click isnt a mine
        if (!hasClicked) {
            int numMines = (int)(graph.GetWidth() * graph.GetHeight() * 0.2);

            graph.GenerateBoard(numMines, n);
            hasClicked = true;
        }
        nview.viewType = ViewType.open;
        graphView.ColorNode(n);

        // display the number of bordering mines if the bordering mines is >0
        int mines = n.CountMines();
        Debug.Log(mines);

        if (mines != 0 && n.nodeType != NodeType.mine) {
            nview.DrawText(mines.ToString());
            Debug.Log("mines.ToString(): " + mines.ToString());
        } else if (mines == 0 && n.nodeType == NodeType.open) {
            // reveal mines surrounding this square
            RevealZeros(n);
        }
    }

    public void FlagNode(Node n) {
        NodeView nview = graphView.nodeViews[n.xIndex, n.yIndex];
        nview.viewType = nview.viewType == ViewType.closed ? ViewType.flagged : ViewType.closed; // toggle between closed and flagged
        nview.DrawText(nview.viewType == ViewType.flagged ? "F" : "");
    }

    public void RevealZeros(Node n) {
        if (n.CountMines() != 0 || n.nodeType != NodeType.open) {
            return;
        }

        foreach (Node neighbor in n.neighbors) {
            NodeView nview = graphView.nodeViews[neighbor.xIndex, neighbor.yIndex];
            if (nview.viewType == ViewType.closed) {
                RevealNode(neighbor);
            }
        }
    }
}
