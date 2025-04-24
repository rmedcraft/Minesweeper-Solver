using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameController gameController;
    public TextMeshProUGUI resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (resultText != null) {
            resultText.text = "";
        } else {
            Debug.Log("UIManager Error: No ResultText Found");
        }
        if (gameController != null) {

            Camera.main.transform.position = new Vector3(gameController.GetGraph().GetWidth() / 2.0f - 0.5f, 1, gameController.GetGraph().GetHeight() / 2.0f - 0.5f);
            Camera.main.orthographicSize = 1f;
            while (!NodesInsideCamera()) {
                Debug.Log("here");
                Camera.main.orthographicSize += 2.0f;
            }

        }
    }

    // returns true if every node is inside the view of the camera
    public bool NodesInsideCamera() {
        foreach (Node n in gameController.GetGraph().nodes) {
            Vector3 nodePos = Camera.main.WorldToViewportPoint(n.position);
            if (nodePos.x < 0 || nodePos.x > 1 || nodePos.y < 0 || nodePos.y > 1) {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnReset() {
        if (resultText != null) {
            resultText.text = "";
        }
    }
}
