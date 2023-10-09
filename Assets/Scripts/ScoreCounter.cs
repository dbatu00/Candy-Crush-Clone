using UnityEngine;
using TMPro;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }
    private int _score = 0;
    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return;

            _score = value;

            scoreText.SetText($"Score = {_score}");
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    private void Awake()
    {
        Instance = this;

        if (scoreText == null)
        {
            scoreText = GetComponent<TextMeshProUGUI>();

            if (scoreText == null)
            {
                Debug.LogError("ScoreText is not assigned in ScoreCounter, and no TextMeshProUGUI component found on the GameObject.");
            }
        }
    }
}
