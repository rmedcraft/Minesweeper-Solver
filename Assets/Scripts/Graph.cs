using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    // Translates 1's and 0's from MapData.cs to an array of nodes
    public Node[,] nodes; //Array of nodes
    public List<Node> walls = new List<Node>();

    int[,] m_mapData;
    int width = 12;
    int height = 7;

    public static readonly Vector2[] allDirections = {
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(1f, -1f),
        new Vector2(0f, 1f),
        new Vector2(0f, -1f),
        new Vector2(-1f, 1f),
        new Vector2(-1f, 0f),
        new Vector2(-1f, -1f),
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    public int GetWidth() {
        return width;
    }
    public int GetHeight() {
        return height;
    }

    public void GenerateBoard(int mines) {
        // for (int r = 0; r < width; r++) {
        //     for (int c = 0; c < height; c++) {
        //         NodeType nodeType = NodeType.open;
        //         if (Random.Range(0, 5) == 0) {
        //             nodeType = NodeType.mine;
        //         }
        //         nodes[r, c] = new Node(r, c, nodeType);
        //     }
        // }

        for (int i = 0; i < mines; i++) {
            int randRow = Random.Range(0, width);
            int randCol = Random.Range(0, height);

            // keep generating random numbers until we get a pos without a mine
            while (nodes[randRow, randCol].nodeType == NodeType.mine) {
                randRow = Random.Range(0, width);
                randCol = Random.Range(0, height);
            }

            nodes[randRow, randCol].nodeType = NodeType.mine;
        }
    }

    public void Init() {
        nodes = new Node[width, height];

        for (int r = 0; r < width; r++) {
            for (int c = 0; c < height; c++) {
                nodes[r, c] = new Node(r, c, NodeType.open);
            }
        }

        int numMines = (int)(width * height * 0.2);
        GenerateBoard(numMines);

        // count the mines bordering each node after all nodes are declared as a mine or not
        for (int r = 0; r < width; r++) {
            for (int c = 0; c < height; c++) {
                nodes[r, c].neighbors = GetNeighbors(r, c, nodes);
            }
        }
    }

    public int CountMines() {
        int mines = 0;
        foreach (Node n in nodes) {
            if (n.nodeType == NodeType.mine) {
                mines++;
            }
        }
        return mines;
    }

    public bool IsWithinBounds(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public List<Node> GetNeighbors(int x, int y, Node[,] nodeArray) {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2 dir in allDirections) {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;
            if (IsWithinBounds(newX, newY) && nodeArray[newX, newY] != null) {
                Debug.Log("Neighbor added to Node[" + x + ", " + y + "]");
                neighbors.Add(nodeArray[newX, newY]);
            }
        }

        return neighbors;
    }
}