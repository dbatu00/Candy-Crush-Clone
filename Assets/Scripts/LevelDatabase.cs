using UnityEngine;
using System.IO;
using System.Linq;

public class LevelDatabase : MonoBehaviour
{
    public static LevelDatabase Instance;
    public static Level[] levelList = new Level[25];
    
    bool debug = false;
    private Level level;

    private void Start()
    {
        if (UniversalVariables.Instance.placeholderLevelListPopulated == false)
        {
            for (int i = 0; i < 25; i++)
            {
                Level newLevel = new Level
                {
                    level_number = i+1,
                    grid_width = 0,
                    grid_height = 0,
                    move_count = 0,
                    grid = new string[0], // Empty grid
                    highScore = 0
                };

                levelList[i] = (newLevel);
            }
            UniversalVariables.Instance.placeholderLevelListPopulated = true;
        }

        if (UniversalVariables.Instance.levelListPopulated == false)
        {
            populateLevelList(0);
            UniversalVariables.Instance.levelListPopulated = true;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //1-a) no internet first execution
    //1-b) no internet 2nd execution (2nd execution does not take place without internet)

    //2-a) no internet first execution
    //2-b) internet second execution

    //3-a) internet first execution
    public void populateLevelList(int index)
    {
        string levelsDirectoryPath = Path.Combine(Application.dataPath, "Levels");

        Debug.Log($"levelsDirectoryPath = {levelsDirectoryPath}");


        if (Directory.Exists(levelsDirectoryPath))
        {
            string[] levelFiles = Directory.GetFiles(levelsDirectoryPath);
            levelFiles = levelFiles.OrderBy(GetLevelNumberFromFileName).ToArray();

            foreach (string filePath in levelFiles)
            {
                if (!filePath.Contains(".meta"))
                {
                    string fileContents = File.ReadAllText(filePath);

                    if(UniversalVariables.Instance.levelListPopulated == true &&
                        ParseLevelData(fileContents).level_number <= 10)
                    {
                        continue;
                    }

                    levelList[index] = ParseLevelData(fileContents); 
                    index++;

                    if (debug)
                    {
                        System.Console.WriteLine($"LVL no: {levelList[0].level_number}\n" +
                                                 $"Width: {levelList[0].grid_width}\n" +
                                                 $"Length: {levelList[0].grid_height}\n" +
                                                 $"Moves: {levelList[0].move_count}\n" +
                                                 $"Grid:{levelList[0].grid}");                    
                    }
                }                              
            }
        }
        else
        {
            Debug.Log("The 'levels' directory does not exist.");
        }
    }
    

    /*
    public void populateLevelList(int index)
    {
        TextAsset[] levelTextAssets = Resources.LoadAll<TextAsset>("Levels");

        if (levelTextAssets != null)
        {
            // Sort the assets by level number
            levelTextAssets = levelTextAssets.OrderBy(asset => GetLevelNumberFromFileName(asset.name)).ToArray();

            foreach (TextAsset textAsset in levelTextAssets)
            {
                string fileContents = textAsset.text;

                if (UniversalVariables.Instance.levelListPopulated == true &&
                    ParseLevelData(fileContents).level_number <= 10)
                {
                    continue;
                }

                levelList[index] = ParseLevelData(fileContents);
                index++;

                if (debug)
                {
                    Debug.Log($"LVL no: {levelList[0].level_number}\n" +
                             $"Width: {levelList[0].grid_width}\n" +
                             $"Length: {levelList[0].grid_height}\n" +
                             $"Moves: {levelList[0].move_count}\n" +
                             $"Grid:{levelList[0].grid}");
                }
            }
        }
        else
        {
            Debug.Log("No text assets found in the 'Levels' folder.");
        }
    }
    */

    private static int GetLevelNumberFromFileName(string filename)
    {
        bool Alevel = true;
        string justFileName = Path.GetFileNameWithoutExtension(filename);
        int lastUnderscoreIndex = justFileName.LastIndexOf('_');
        if (justFileName[lastUnderscoreIndex +1] != 'A')
        {
            Alevel = false;
        }

        string numberPart = justFileName.Substring(lastUnderscoreIndex + 2);

        if (int.TryParse(numberPart, out int result))
        {
            if(Alevel == false)
            {
                result = result + 15;
            }
            return result;
        }
        else
        {
            Debug.Log("Error getting level number from filename");
            return -1;
        }
    }

    private Level ParseLevelData(string levelAsset)
    {
        level = new Level();
        string[] lines = levelAsset.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (line.StartsWith("level_number:"))
            {
                level.level_number = int.Parse(line.Substring("level_number:".Length).Trim());
            }
            else if (line.StartsWith("grid_width:"))
            {
                level.grid_width = int.Parse(line.Substring("grid_width:".Length).Trim());
            }
            else if (line.StartsWith("grid_height:"))
            {
                level.grid_height = int.Parse(line.Substring("grid_height:".Length).Trim());
            }
            else if (line.StartsWith("move_count:"))
            {
                level.move_count = int.Parse(line.Substring("move_count:".Length).Trim());
            }
            else if (line.StartsWith("grid:"))
            {
                string gridData = line.Substring("grid:".Length).Trim();
                level.grid = orderGrid(gridData.Split(','));
            }
        }

        if (debug)
        {
            Debug.Log("Level Number: " + level.level_number);
            Debug.Log("Grid Width: " + level.grid_width);
            Debug.Log("Grid Height: " + level.grid_height);
            Debug.Log("Move Count: " + level.move_count);
            Debug.Log("Grid Data: ");
            foreach (string cell in level.grid)
            {
                Debug.Log(cell);
            }
        }
       
        return level;
    }

    private string[] orderGrid(string[] original)
    {
        string[] ordered = new string[original.Length];
        int index = 0;

        for(int i = level.grid_height -1; i>=0; i--)
        {
            for(int j = 0; j<level.grid_width; j++)
            {
                ordered[index] = original[(i * level.grid_width) + j];
                index++;
            }
        }

        return ordered;
    }
}

[System.Serializable]
public class Level
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid; // bottom left to top right
    public int highScore = 0;
}



