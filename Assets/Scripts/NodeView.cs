using UnityEngine;

public class NodeView : MonoBehaviour {
    public GameObject tile;
    float borderSize = 0.1f;

    TextMesh text;
    Node node;
    public ViewType viewType = ViewType.closed;

    Minesweeper minesweeper;
    public void Init(Node node, GameController gameController) {
        if (tile != null) {
            this.node = node;
            this.minesweeper = gameController.minesweeper;
            // gameObject refers to the NodeView gameObject
            // gameObject is kinda like saying this.something() in every other programming language
            tile.name = "Node (" + node.position.x + ", " + node.position.z + ")";
            tile.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);

            // Adds a hitbox to each tile, necessary for click detection
            tile.AddComponent<BoxCollider>();

            // all for adding text to the nodeview
            GameObject t = new GameObject();
            text = t.AddComponent<TextMesh>();
            text.text = "";
            text.fontSize = 80;
            text.characterSize = 0.1f;
            text.color = Color.black;

            text.transform.position = node.position;

            text.transform.eulerAngles = new Vector3(90, 0, 0);
            text.anchor = TextAnchor.MiddleCenter;
        } else {
            Debug.LogWarning("Tile does not exist!");
        }
    }

    void ColorNode(Color color, GameObject gameObject) {
        if (gameObject != null) {
            Renderer gameObjectRenderer = gameObject.GetComponent<Renderer>();
            gameObjectRenderer.material.color = color;
        }
    }

    public void ColorNode(Color color) {
        ColorNode(color, tile);
    }

    void Update() {
        // handles click detection for each individual node with a raycaster
        // cant use the OnMouseDown method because we are using tile as the nodeview object.
        // you can only change a node state manually if the game is paused
        if (minesweeper.GetPlaying() && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // out hit means to store the output in the hit object
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject == tile) {
                    if (Input.GetMouseButtonDown(0)) {
                        if (viewType == ViewType.closed) {
                            minesweeper.RevealNode(node);
                        } else if (viewType == ViewType.open) {
                            minesweeper.ClearFlagged(node);
                        }
                    } else {
                        minesweeper.FlagNode(node);
                    }
                }
            }
        }
    }

    public void DrawText(string s) {
        text.text = s;
    }
}

public enum ViewType {
    open = 0,
    closed = 1,
    flagged = 2
}