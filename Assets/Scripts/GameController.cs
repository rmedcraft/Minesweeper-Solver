using UnityEngine;

public class GameController : MonoBehaviour {
    public Graph graph;
    public BoardSolver boardSolver;
    public Minesweeper minesweeper;
    public UIManager ui;
    public float timeStep = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (graph != null) {
            graph.Init();
            GraphView graphView = graph.GetComponent<GraphView>();
            if (graphView != null) {
                graphView.Init(graph, this);
            } else {
                Debug.Log("GameController Error: No graph view is found");
            }

            if (minesweeper != null) {
                minesweeper.Init(graph, this);
            } else {
                Debug.Log("GameController Error: No Minesweeper object found");
            }

            if (boardSolver != null) {
                boardSolver.Init(graph, this);
            }


            // if (graph.IsWithinBounds(startx, starty) && pathfinder != null) {
            //     Node startNode = graph.nodes[startx, starty];
            //     Node goalNode = graph.nodes[goalx, goaly];
            //     pathfinder.Init(graph, graphView, startNode, goalNode);
            //     StartCoroutine(pathfinder.SearchRoutine(timeStep));
            // } else {
            //     Debug.LogWarning("GameController Error: start or end nodes are not in bounds");
            // }
        }
    }

    public Graph GetGraph() {
        return graph;
    }

    // Update is called once per frame
    void Update() {

    }
}
