using UnityEngine;
using TMPro;

public class MoveCounter : MonoBehaviour
{
    public static MoveCounter Instance { get; private set; }  
    private int _moves = 0;
    public int Moves
    {
        get => _moves;

        set
        {
            if (_moves == value) return;

            _moves = value;

            movesText.SetText($"Moves  = {_moves}");
        }
    }

    [SerializeField] private TextMeshProUGUI movesText;
    private void Awake()
    {
        Instance = this;

        if (movesText == null)
        {
            movesText = GetComponent<TextMeshProUGUI>();

            if (movesText == null)
            {
                Debug.LogError("movesText is not assigned in moveCounter, and no TextMeshProUGUI component found on the GameObject.");
            }
        }
    }
}
