using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    private TMP_Text textMesh;
    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (textMesh != null) { textMesh.SetText(GameState.Instance.CurrentScore); }

    }
}

