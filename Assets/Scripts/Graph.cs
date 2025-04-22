using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    // Translates 1's and 0's from MapData.cs to an array of nodes
    public Node[,] nodes; //Array of nodes
    public List<Node> nodeList = new List<Node>();
    int nodeIndex = 1;

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

    public void GenerateBoard(int mines, Node start) {
        // old solution, O(n) with repeated randomization
        // for (int i = 0; i < mines; i++) {
        //     int randRow = Random.Range(0, width);
        //     int randCol = Random.Range(0, height);

        //     Node currentNode = nodes[randRow, randCol];
        //     // keep generating random numbers until we get a pos without a mine & that isnt bordering the first click
        //     while (currentNode.nodeType == NodeType.mine || start == currentNode) {
        //         randRow = Random.Range(0, width);
        //         randCol = Random.Range(0, height);
        //     }

        //     nodes[randRow, randCol].nodeType = NodeType.mine;
        // }

        // new solution, O(n) no repeated randomization
        // this method needs to be really efficient if its being called after the first click
        Swap(nodeList, 0, nodeList.IndexOf(start));
        foreach (Node n in start.neighbors) {
            Swap(nodeList, nodeIndex, nodeList.IndexOf(n, nodeIndex));
            nodeIndex++;
        }
        for (int i = 0; i < mines; i++) {
            int randIndex = Random.Range(nodeIndex, nodeList.Count);
            nodeList[randIndex].nodeType = NodeType.mine;
            Swap(nodeList, nodeIndex, randIndex);
            nodeIndex++;
        }
    }

    void Swap(List<Node> list, int i1, int i2) {
        Node temp = list[i1];
        list[i1] = list[i2];
        list[i2] = temp;
    }


    public void Init() {
        nodes = new Node[width, height];

        for (int r = 0; r < width; r++) {
            for (int c = 0; c < height; c++) {
                nodes[r, c] = new Node(r, c, NodeType.open);
                nodeList.Add(nodes[r, c]);
            }
        }

        // declare the neighbor list
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