using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;

    /// <summary>
    /// Check if an item was already assigned to this tile.
    /// If not, assign an item and change icon
    /// </summary>
    public Item Item
    {
        get { return _item; }
        set
        {
            if (_item == value) return;

            _item = value;

            icon.sprite = _item.sprite;
        }
    }

    public Image icon;
    public Button button;

    public Tile bottom  => y!=Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;
    public Tile left    => x!=0                         ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile top     => y!=0                         ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile right   => x!= Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;

    public Tile[] neighbours => new[]
    {
        bottom,
        left,
        top,
        right
    };

    /// <summary>
    /// tell the board to select this tile
    /// </summary>
    private void Start() => button.onClick.AddListener(() => Board.Instance.Select(this)); 
}
