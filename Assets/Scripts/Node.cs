using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType {
    open = 0,
    mine = 1,
}

public class Node {
    // this is a single node in a graph
    public NodeType nodeType = NodeType.open;
    public int xIndex = -1;
    public int yIndex = -1;
    public Vector3 position;

    public List<Node> neighbors = new List<Node>();
    public Node prev = null;
    public Node(int xIndex, int yIndex, NodeType nodeType) {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
        position = new Vector3(xIndex, 0, yIndex);
    }

    public void Reset() {
        prev = null;
    }

    public int CountMines() {
        int mineCt = 0;
        foreach (Node n in neighbors) {
            Debug.Log("Neighbor NodeType: " + n.nodeType);
            if (n.nodeType == NodeType.mine) {
                mineCt++;
            }
        }
        return mineCt;
    }
}


