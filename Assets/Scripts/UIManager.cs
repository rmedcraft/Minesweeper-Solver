using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameController gameController;
    public TextMeshProUGUI resultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (resultText != null) {
            resultText.text = "";
        }
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
