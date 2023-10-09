using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Tile[,] Tiles { get; private set; }
    public int level;
    public int Width => LevelDatabase.levelList[level].grid_width;
    public int Height => LevelDatabase.levelList[level].grid_height;

    public GameObject rowPrefab;
    public Transform rowContainer; // board
    public GameObject tilePrefab;
    public Transform tileContainer; // row

    private readonly List<Tile> _selection = new List<Tile>();
    private int movesLeft;
    private const float TweenDuration = 0.25f;

    private void Awake() => Instance = this;

    private void Start()
    {
        Debug.Log($"Level: {level+1} has started being constructed"); //levelList[0] = first level which has level number as 1
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        Debug.Log("Board generation started");
        
        GameManager gameManager = GameManager.instance;
        Debug.Log("GameManager instanced(?)");
        
        level = gameManager._level-1; //-1 for indexing
        Debug.Log($"Level = {level+1}");

        movesLeft = LevelDatabase.levelList[level].move_count;
        MoveCounter.Instance.Moves = movesLeft;
        ScoreCounter.Instance.Score = 0;
        Tiles = new Tile[Width, Height];
        GameObject rowObj;
        GameObject tileObj;

        int index = 0;


        for (int i = 0; i <Height; i++) //populate and instantiate tiles and rows
        {
            rowObj = Instantiate(rowPrefab, transform);
        
            for (int j = 0; j < Width; j++)
            {
                tileObj = Instantiate(tilePrefab, rowObj.transform);
                
                Tile tile = tileObj.GetComponent<Tile>();
                if (LevelDatabase.levelList[level].grid[index] == "r") tile.Item = Resources.Load<Item>("Items/kirmizi");
                else if (LevelDatabase.levelList[level].grid[index] == "b") tile.Item = Resources.Load<Item>("Items/mavi");
                else if (LevelDatabase.levelList[level].grid[index] == "y") tile.Item = Resources.Load<Item>("Items/sari");
                else if (LevelDatabase.levelList[level].grid[index] == "g") tile.Item = Resources.Load<Item>("Items/yesil");
                else Debug.Log("Something is wrong in population of the board");

                index++;

                tile.x = j;
                tile.y = i;

                Tiles[j,i] = tile;                    
            }
        }
    }

   

    public async void Select(Tile tile)
    {
        //add tile to selection if its not already in selection and is not a popped tile
        if (!_selection.Contains(tile) && tile.Item.value != -1) { _selection.Add(tile); }
        else Debug.Log($"Could not select tile at ({tile.x}, {tile.y}) because it is already selected or is a done tile");

        if (_selection.Count < 2) return;

        Debug.Log($"Selected tiles at({_selection[0].x}, {_selection[0].y})" +
                               $"and ({_selection[1].x}, {_selection[1].y})");

        if (_selection[0].neighbours.Contains(_selection[1])) //tile2 is adjacent to tile1
        {
            await Swap(_selection[0], _selection[1]);
            movesLeft--;
            MoveCounter.Instance.Moves = movesLeft;
            if (movesLeft == 0)
            {
                if (ScoreCounter.Instance.Score > LevelDatabase.levelList[level].highScore)
                {
                    LevelDatabase.levelList[level].highScore = ScoreCounter.Instance.Score;
                    Debug.Log($"New High Score on level{level + 1}: {ScoreCounter.Instance.Score}");
                    UniversalVariables.Instance.playAnimation = true;          
                    SceneManager.LoadScene("LevelSelection");
                    level = -2;
                    return;
                }
                else
                {
                    SceneManager.LoadScene("LevelSelection");
                    level = -2;
                    return;
                }
            }
            else if (!CanPopAnyRow())
            {
                if (ScoreCounter.Instance.Score > LevelDatabase.levelList[level].highScore)
                {
                    LevelDatabase.levelList[level].highScore = ScoreCounter.Instance.Score;
                    Debug.Log($"New High Score on level{level + 1}: {ScoreCounter.Instance.Score}");
                    UniversalVariables.Instance.playAnimation = true;                 
                    SceneManager.LoadScene("LevelSelection");
                    level = -2;
                    return;
                }
                else
                {
                    SceneManager.LoadScene("LevelSelection");
                    level = -2;
                    return;
                }
            }

            TryPopRow(_selection[0], _selection[1]);

            if (!CanPopAnyRow()) //check after popping tp see if no pops left
            {
                if (ScoreCounter.Instance.Score > LevelDatabase.levelList[level].highScore)
                {
                    LevelDatabase.levelList[level].highScore = ScoreCounter.Instance.Score;
                    Debug.Log($"New High Score on level{level + 1}: {ScoreCounter.Instance.Score}");
                    UniversalVariables.Instance.playAnimation = true;
                    SceneManager.LoadScene("LevelSelection");
                    level = -2;
                    return;
                }
                else
                {
                    SceneManager.LoadScene("LevelSelection");
                    level = -2;
                    return;
                }
            }
        }
        else
        {
            Thread.Sleep(1000); // to let the message be read, saðlýklý çözüm deðil farkýndayým ama conditional directivelerle uðraþmak istemedim
            Debug.Log("Selected tiles are not neighbours");
        }

        _selection.Clear();
    }

    /// <summary>
    /// Check to see if there are no item type where item count >= Width
    /// Which is the case when there is no possible tile combination that allows a pop
    /// ToDo: Item types can be parameterized/contained
    /// ToDo2: more sophisticated checking algorithms can be added but it will slow the game down
    /// </summary>
    private bool CanPopAnyRow()
    {
        bool canPop = true;
        int redCount = 0;
        int greenCount = 0;
        int blueCount = 0;
        int yellowCount = 0;

        for(int i=0; i<Width; i++)
        {
            for(int j=0; j<Height; j++)
            {
                if (Tiles[i, j].Item == Resources.Load<Item>("Items/kirmizi")) redCount++;
                else if (Tiles[i, j].Item == Resources.Load<Item>("Items/sari")) yellowCount++;
                else if (Tiles[i, j].Item == Resources.Load<Item>("Items/mavi")) blueCount++;
                else if (Tiles[i, j].Item == Resources.Load<Item>("Items/yesil")) greenCount++;
                else if (Tiles[i, j].Item == Resources.Load<Item>("Items/tamam")) ;
                else { Debug.Log("Something is wrong in counting"); }
            }
        }

        if (redCount < Width && blueCount < Width && yellowCount < Width && greenCount < Width) canPop = false;

        return canPop;
    }


    //ToDo: Bugs when selection is too fast, could have to do with select function
    public async Task Swap(Tile tile1, Tile tile2)
    {
        var icon1 = tile1.icon;
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play()
                      .AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2;
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;
    }
    
    /// <summary>
    /// Check if any of the rows of the swapped tiles have homogenous shapes(checked via values)
    /// ToDo: no need to check for vertical swaps
    /// ToDo2: can be made to wait for both PopRows to finish before updating the board
    /// </summary>
    private void TryPopRow(Tile tile1, Tile tile2)
    {
        bool canPopTile1Row = true;
        bool canPopTile2Row = true;

        int row;
        int i;
       
        row = tile1.y;
        for (i=0; i<Width; i++) //take the shape of the moved tile 
                                //If there are any different items, canPopTile becomes false
                                //If all items are the same, bool stays true and initiates PopRow()
        {
            if (Tiles[i, row].Item.value != tile1.Item.value) canPopTile1Row = false;
        }

        if (canPopTile1Row) PopRow(row);

        row = tile2.y;
        for (i = 0; i < Width; i++)                         
        {
            if (Tiles[i, row].Item.value != tile2.Item.value) canPopTile2Row = false;
        }

        if (canPopTile2Row) PopRow(row);
    }

    private void PopRow(int row)
    {
        ScoreCounter.Instance.Score += Tiles[0, row].Item.value * Width;
        Item item;

        for (int i=0; i<Width; i++)
        {
            item = Resources.Load<Item>("Items/tamam");
            Tiles[i, row].Item = item;
        }       
    }
}
