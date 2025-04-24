using UnityEngine;

public class GameController : MonoBehaviour {
    public Graph graph;
    public BoardSolver boardSolver;
    public Minesweeper minesweeper;
    public UIManager ui;
    public int startx = 0;
    public int starty = 0;
    public float timeStep = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (graph != null) {
            graph.Init();
            GraphView graphView = graph.GetComponent<GraphView>();
            if (graphView != null) {
                if (minesweeper != null) {
                    minesweeper.Init(graph, this);
                    graphView.Init(graph, this);
                } else {
                    Debug.Log("GameController Error: No Minesweeper object found");
                }
            } else {
                Debug.Log("GameController Error: No graph view is found");
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

    public Minesweeper GetMinesweeper() {
        return minesweeper;
    }

    public BoardSolver GetBoardSolver() {
        return boardSolver;
    }

    public UIManager GetUI() {
        return ui;
    }

    // Update is called once per frame
    void Update() {

    }
}
