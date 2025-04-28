using System.Collections.Generic;
using UnityEngine;

public class GraphView : MonoBehaviour {
    public GameObject nodeViewPrefab;
    public Color closedColor = Color.gray;
    public Color openColor = Color.white;
    public Color mineColor = Color.red;
    public NodeView[,] nodeViews;

    public void Init(Graph graph, GameController gameController) {
        if (graph == null) {
            Debug.LogWarning("Graph does not exist u stupid idiot");
            return;
        }

        nodeViews = new NodeView[graph.GetWidth(), graph.GetHeight()];
        foreach (Node n in graph.nodes) {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();
            if (nodeView != null) {
                nodeView.Init(n, gameController);
                nodeViews[n.xIndex, n.yIndex] = nodeView;
                nodeView.ColorNode(closedColor);

                // shows the mines, for debug purposes
                // if (n.nodeType == NodeType.mine) {
                //     nodeView.ColorNode(mineColor);
                // }
            }
        }
    }



    public void ColorNodes(List<Node> nodes, Color color) {
        foreach (Node n in nodes) {
            if (n != null) {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if (nodeView != null) {
                    nodeView.ColorNode(color);
                }
            }
        }
    }

    // colors the node based on its state
    public void ColorNode(Node n) {
        NodeView nview = nodeViews[n.xIndex, n.yIndex];
        nview.ColorNode(n.nodeType == NodeType.open ? openColor : mineColor);
    }
}
